using System.Text.Json;
using CorpusMiner.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace CorpusMiner.Web.Services.CorpusIngestion;

/// <summary>
/// Parsea un archivo subido y fusiona sus registros dentro del Corpus: por Doi si esta presente,
/// si no por Titulo normalizado + Anio. Cada registro fuente queda ademas guardado como
/// PaperSourceRecord (evidencia) sin importar si genero un Paper nuevo o se fusiono en uno existente.
/// </summary>
public sealed class CorpusIngestionService(ApplicationDbContext dbContext, IEnumerable<IBibliographicParser> parsers)
{
    private const int FlushEvery = 500;

    public async Task IngestAsync(CorpusSourceFile sourceFile, Stream content, CancellationToken cancellationToken = default)
    {
        var parser = parsers.FirstOrDefault(p => p.SupportedType == sourceFile.SourceType)
            ?? throw new InvalidOperationException($"No hay parser registrado para {sourceFile.SourceType}.");

        sourceFile.Status = CorpusSourceFileStatus.Parsing;
        await dbContext.SaveChangesAsync(cancellationToken);

        try
        {
            var parsedRecords = parser.Parse(content);

            var existingPapers = await dbContext.Papers
                .Where(p => p.CorpusId == sourceFile.CorpusId)
                .ToListAsync(cancellationToken);

            var byDoi = existingPapers
                .Where(p => !string.IsNullOrWhiteSpace(p.Doi))
                .ToDictionary(p => p.Doi!, StringComparer.OrdinalIgnoreCase);
            var byTitleYear = existingPapers
                .ToDictionary(p => (p.Title.Trim().ToLowerInvariant(), p.Year));

            var processed = 0;
            foreach (var parsed in parsedRecords)
            {
                var paper = FindMatch(parsed, byDoi, byTitleYear);
                if (paper is null)
                {
                    paper = new Paper
                    {
                        CorpusId = sourceFile.CorpusId,
                        Title = parsed.Title,
                        Doi = parsed.Doi,
                        Year = parsed.Year,
                        SourceTitle = parsed.SourceTitle,
                        Abstract = parsed.Abstract,
                        RawMetadataJson = BuildRawMetadataJson(sourceFile.SourceType, parsed.RawFields),
                    };
                    dbContext.Papers.Add(paper);

                    if (!string.IsNullOrWhiteSpace(paper.Doi))
                    {
                        byDoi[paper.Doi] = paper;
                    }
                    byTitleYear[(paper.Title.Trim().ToLowerInvariant(), paper.Year)] = paper;
                }
                else
                {
                    MergeInto(paper, sourceFile.SourceType, parsed);
                }

                dbContext.PaperSourceRecords.Add(new PaperSourceRecord
                {
                    Paper = paper,
                    CorpusSourceFileId = sourceFile.Id,
                    RawFieldsJson = JsonSerializer.Serialize(parsed.RawFields),
                });

                processed++;
                if (processed % FlushEvery == 0)
                {
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }

            sourceFile.Status = CorpusSourceFileStatus.Parsed;
            sourceFile.RecordCount = parsedRecords.Count;
        }
        catch (Exception ex)
        {
            sourceFile.Status = CorpusSourceFileStatus.Failed;
            sourceFile.ErrorMessage = ex.Message;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static Paper? FindMatch(
        ParsedPaperRecord parsed,
        Dictionary<string, Paper> byDoi,
        Dictionary<(string Title, int? Year), Paper> byTitleYear)
    {
        if (!string.IsNullOrWhiteSpace(parsed.Doi) && byDoi.TryGetValue(parsed.Doi, out var matchByDoi))
        {
            return matchByDoi;
        }

        return byTitleYear.GetValueOrDefault((parsed.Title.Trim().ToLowerInvariant(), parsed.Year));
    }

    private static void MergeInto(Paper paper, CorpusSourceType sourceType, ParsedPaperRecord parsed)
    {
        paper.Doi ??= parsed.Doi;
        paper.Year ??= parsed.Year;
        paper.SourceTitle ??= parsed.SourceTitle;
        paper.Abstract ??= parsed.Abstract;

        var metadata = DeserializeMetadata(paper.RawMetadataJson);
        metadata[sourceType.ToString()] = parsed.RawFields;
        paper.RawMetadataJson = JsonSerializer.Serialize(metadata);
    }

    private static string BuildRawMetadataJson(CorpusSourceType sourceType, IReadOnlyDictionary<string, string?> rawFields)
    {
        var metadata = new Dictionary<string, IReadOnlyDictionary<string, string?>>
        {
            [sourceType.ToString()] = rawFields,
        };
        return JsonSerializer.Serialize(metadata);
    }

    private static Dictionary<string, IReadOnlyDictionary<string, string?>> DeserializeMetadata(string json)
    {
        return JsonSerializer.Deserialize<Dictionary<string, IReadOnlyDictionary<string, string?>>>(json) ?? [];
    }
}
