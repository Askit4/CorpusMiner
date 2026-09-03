namespace CorpusMiner.Web.Data;

/// <summary>
/// Registro bibliografico combinado dentro de un Corpus. Se fusiona entre fuentes por
/// Doi (si esta presente) o por Titulo+Anio. Los campos que aun no se normalizan
/// (autores, journal detallado, keywords, referencias citadas) viven en RawMetadataJson,
/// una entrada por CorpusSourceType que contribuyo al registro.
/// </summary>
public class Paper
{
    public int Id { get; set; }

    public int CorpusId { get; set; }

    public Corpus? Corpus { get; set; }

    public required string Title { get; set; }

    public string? Doi { get; set; }

    public int? Year { get; set; }

    public string? SourceTitle { get; set; }

    public string? Abstract { get; set; }

    /// <summary>JSON: {"WebOfScience": {...}, "Scopus": {...}}.</summary>
    public string RawMetadataJson { get; set; } = "{}";

    public List<PaperSourceRecord> SourceRecords { get; set; } = [];
}
