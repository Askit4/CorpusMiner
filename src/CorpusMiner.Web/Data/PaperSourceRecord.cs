namespace CorpusMiner.Web.Data;

/// <summary>
/// Evidencia: los campos crudos exactos que un archivo fuente aporto a un Paper,
/// antes de la fusion. Permite trazar cualquier resultado de analisis de vuelta
/// al registro original de WoS/Scopus.
/// </summary>
public class PaperSourceRecord
{
    public int Id { get; set; }

    public int PaperId { get; set; }

    public Paper? Paper { get; set; }

    public int CorpusSourceFileId { get; set; }

    public CorpusSourceFile? CorpusSourceFile { get; set; }

    public required string RawFieldsJson { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}
