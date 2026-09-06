using CorpusMiner.Web.Data;

namespace CorpusMiner.Web.Services.CorpusIngestion;

/// <summary>Parsea un archivo bibliografico crudo de una fuente especifica (WoS, Scopus, ...).</summary>
public interface IBibliographicParser
{
    CorpusSourceType SupportedType { get; }

    IReadOnlyList<ParsedPaperRecord> Parse(Stream content);
}
