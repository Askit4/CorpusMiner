namespace CorpusMiner.Web.Data;

/// <summary>Proyecto de investigacion: contenedor de uno o mas Corpus.</summary>
public class MiningResearch
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public required string CreatedByUserId { get; set; }

    public ApplicationUser? CreatedBy { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public List<Corpus> Corpora { get; set; } = [];
}
