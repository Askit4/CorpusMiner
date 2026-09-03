namespace CorpusMiner.Web.Services.CorpusIngestion;

/// <summary>Un registro bibliografico extraido de un archivo fuente, antes de fusionarse en el Corpus.</summary>
public sealed class ParsedPaperRecord
{
    public required string Title { get; init; }

    public string? Doi { get; init; }

    public int? Year { get; init; }

    public string? SourceTitle { get; init; }

    public string? Abstract { get; init; }

    public required IReadOnlyDictionary<string, string?> RawFields { get; init; }
}
