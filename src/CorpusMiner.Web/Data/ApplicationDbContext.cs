using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CorpusMiner.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Dataset> Datasets => Set<Dataset>();

    public DbSet<DatasetComment> DatasetComments => Set<DatasetComment>();

    public DbSet<DatasetLike> DatasetLikes => Set<DatasetLike>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Dataset>()
            .HasOne(d => d.CreatedBy)
            .WithMany()
            .HasForeignKey(d => d.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<DatasetComment>()
            .HasOne(c => c.Dataset)
            .WithMany(d => d.Comments)
            .HasForeignKey(c => c.DatasetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<DatasetComment>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<DatasetLike>()
            .HasOne(l => l.Dataset)
            .WithMany(d => d.Likes)
            .HasForeignKey(l => l.DatasetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<DatasetLike>()
            .HasOne(l => l.User)
            .WithMany()
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<DatasetLike>()
            .HasIndex(l => new { l.DatasetId, l.UserId })
            .IsUnique();
    }
}
