namespace CorpusMiner.Web.Data;

/// <summary>Estado del procesamiento de un archivo subido a un Corpus.</summary>
public enum CorpusSourceFileStatus
{
    Uploaded,
    Parsing,
    Parsed,
    Failed,
}
