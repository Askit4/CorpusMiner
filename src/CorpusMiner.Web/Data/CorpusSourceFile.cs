namespace CorpusMiner.Web.Data;

/// <summary>Un archivo bibliografico crudo (WoS/Scopus/...) subido a un Corpus.</summary>
public class CorpusSourceFile
{
    public int Id { get; set; }

    public int CorpusId { get; set; }

    public Corpus? Corpus { get; set; }

    public required string OriginalFileName { get; set; }

    public CorpusSourceType SourceType { get; set; }

    /// <summary>Ruta dentro del contenedor blob "corpus-raw-uploads".</summary>
    public required string BlobPath { get; set; }

    public required string UploadedByUserId { get; set; }

    public ApplicationUser? UploadedBy { get; set; }

    public DateTimeOffset UploadedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public CorpusSourceFileStatus Status { get; set; } = CorpusSourceFileStatus.Uploaded;

    public int? RecordCount { get; set; }

    public string? ErrorMessage { get; set; }
}
