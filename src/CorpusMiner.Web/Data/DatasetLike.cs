namespace CorpusMiner.Web.Data;

/// <summary>"Like" de un usuario sobre un dataset. Un usuario solo puede dar like una vez por dataset.</summary>
public class DatasetLike
{
    public int Id { get; set; }

    public int DatasetId { get; set; }

    public Dataset? Dataset { get; set; }

    public required string UserId { get; set; }

    public ApplicationUser? User { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}
