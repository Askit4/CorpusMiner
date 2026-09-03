using System.Text;
using CorpusMiner.Web.Data;

namespace CorpusMiner.Web.Services.CorpusIngestion;

/// <summary>
/// Parser del export "Full Record" (texto plano con tags) de Web of Science: cada campo es
/// una linea "XX contenido" (tag de 2 letras + espacio + valor), las lineas de continuacion
/// van indentadas con 3 espacios, y "ER" en su propia linea termina el registro actual.
/// </summary>
public sealed class WebOfScienceTaggedParser : IBibliographicParser
{
    public CorpusSourceType SupportedType => CorpusSourceType.WebOfScience;

    public IReadOnlyList<ParsedPaperRecord> Parse(Stream content)
    {
        var records = new List<ParsedPaperRecord>();
        var currentFields = new Dictionary<string, StringBuilder>();
        string? currentTag = null;

        using var reader = new StreamReader(content);
        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            if (line.Length == 0)
            {
                continue;
            }

            if (line.StartsWith("   ", StringComparison.Ordinal))
            {
                if (currentTag is not null && currentFields.TryGetValue(currentTag, out var continuation))
                {
                    continuation.Append(' ').Append(line.Trim());
                }
                continue;
            }

            var tag = line.Length >= 2 ? line[..2] : line;

            if (tag == "ER")
            {
                if (currentFields.Count > 0)
                {
                    records.Add(BuildRecord(currentFields));
                }
                currentFields = [];
                currentTag = null;
                continue;
            }

            if (tag is "EF" or "FN" or "VR")
            {
                continue;
            }

            var value = line.Length > 3 ? line[3..] : string.Empty;
            currentTag = tag;
            if (!currentFields.TryGetValue(tag, out var buffer))
            {
                buffer = new StringBuilder();
                currentFields[tag] = buffer;
            }
            else
            {
                buffer.Append("; ");
            }

            buffer.Append(value);
        }

        return records;
    }

    private static ParsedPaperRecord BuildRecord(Dictionary<string, StringBuilder> fields)
    {
        var byTag = fields.ToDictionary(kv => kv.Key, kv => (string?)kv.Value.ToString());

        byTag.TryGetValue("PY", out var yearRaw);
        int? year = int.TryParse(yearRaw, out var parsedYear) ? parsedYear : null;

        var doi = byTag.GetValueOrDefault("DI");

        return new ParsedPaperRecord
        {
            Title = byTag.GetValueOrDefault("TI") is { Length: > 0 } title ? title : "(sin titulo)",
            Doi = string.IsNullOrWhiteSpace(doi) ? null : doi.Trim(),
            Year = year,
            SourceTitle = byTag.GetValueOrDefault("SO"),
            Abstract = byTag.GetValueOrDefault("AB"),
            RawFields = byTag,
        };
    }
}
