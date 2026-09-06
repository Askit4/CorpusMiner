namespace CorpusMiner.Web.Data;

public class DatasetComment
{
    public int Id { get; set; }

    public int DatasetId { get; set; }

    public Dataset? Dataset { get; set; }

    public required string UserId { get; set; }

    public ApplicationUser? User { get; set; }

    public required string Body { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}
