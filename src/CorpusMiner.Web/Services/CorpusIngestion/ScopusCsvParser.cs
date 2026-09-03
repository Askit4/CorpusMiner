using System.Globalization;
using CorpusMiner.Web.Data;
using CsvHelper;
using CsvHelper.Configuration;

namespace CorpusMiner.Web.Services.CorpusIngestion;

/// <summary>
/// Parser del export CSV de Scopus. Las columnas exportadas varian segun lo que el usuario
/// elige en Scopus al exportar, asi que se leen dinamicamente por encabezado en vez de asumir
/// un esquema de columnas fijo.
/// </summary>
public sealed class ScopusCsvParser : IBibliographicParser
{
    public CorpusSourceType SupportedType => CorpusSourceType.Scopus;

    public IReadOnlyList<ParsedPaperRecord> Parse(Stream content)
    {
        var records = new List<ParsedPaperRecord>();

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null,
        };

        using var reader = new StreamReader(content);
        using var csv = new CsvReader(reader, config);

        if (!csv.Read() || !csv.ReadHeader() || csv.HeaderRecord is null)
        {
            return records;
        }

        var headers = csv.HeaderRecord;

        while (csv.Read())
        {
            var rawFields = new Dictionary<string, string?>();
            foreach (var header in headers)
            {
                rawFields[header] = csv.GetField(header);
            }

            var doi = rawFields.GetValueOrDefault("DOI");
            var year = int.TryParse(rawFields.GetValueOrDefault("Year"), out var parsedYear) ? parsedYear : (int?)null;

            records.Add(new ParsedPaperRecord
            {
                Title = rawFields.GetValueOrDefault("Title") is { Length: > 0 } title ? title : "(sin titulo)",
                Doi = string.IsNullOrWhiteSpace(doi) ? null : doi.Trim(),
                Year = year,
                SourceTitle = rawFields.GetValueOrDefault("Source title"),
                Abstract = rawFields.GetValueOrDefault("Abstract"),
                RawFields = rawFields,
            });
        }

        return records;
    }
}
