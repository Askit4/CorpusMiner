namespace CorpusMiner.Web.Data;

/// <summary>Publicacion de un dataset en el foro (metadatos; no el archivo de corpus en si).</summary>
public class Dataset
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public required string CreatedByUserId { get; set; }

    public ApplicationUser? CreatedBy { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public List<DatasetComment> Comments { get; set; } = [];

    public List<DatasetLike> Likes { get; set; } = [];
}
