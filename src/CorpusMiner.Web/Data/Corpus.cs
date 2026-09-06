namespace CorpusMiner.Web.Data;

/// <summary>Base de conocimiento que acumula papers combinados desde uno o mas archivos subidos (WoS/Scopus/...).</summary>
public class Corpus
{
    public int Id { get; set; }

    public int MiningResearchId { get; set; }

    public MiningResearch? MiningResearch { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public List<CorpusSourceFile> SourceFiles { get; set; } = [];

    public List<Paper> Papers { get; set; } = [];
}
