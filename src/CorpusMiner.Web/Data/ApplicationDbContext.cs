using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CorpusMiner.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Dataset> Datasets => Set<Dataset>();

    public DbSet<DatasetComment> DatasetComments => Set<DatasetComment>();

    public DbSet<DatasetLike> DatasetLikes => Set<DatasetLike>();

    public DbSet<MiningResearch> MiningResearches => Set<MiningResearch>();

    public DbSet<Corpus> Corpora => Set<Corpus>();

    public DbSet<CorpusSourceFile> CorpusSourceFiles => Set<CorpusSourceFile>();

    public DbSet<Paper> Papers => Set<Paper>();

    public DbSet<PaperSourceRecord> PaperSourceRecords => Set<PaperSourceRecord>();

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

        builder.Entity<MiningResearch>()
            .HasOne(r => r.CreatedBy)
            .WithMany()
            .HasForeignKey(r => r.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Corpus>()
            .HasOne(c => c.MiningResearch)
            .WithMany(r => r.Corpora)
            .HasForeignKey(c => c.MiningResearchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CorpusSourceFile>()
            .HasOne(f => f.Corpus)
            .WithMany(c => c.SourceFiles)
            .HasForeignKey(f => f.CorpusId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CorpusSourceFile>()
            .HasOne(f => f.UploadedBy)
            .WithMany()
            .HasForeignKey(f => f.UploadedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Paper>()
            .HasOne(p => p.Corpus)
            .WithMany(c => c.Papers)
            .HasForeignKey(p => p.CorpusId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Paper>()
            .HasIndex(p => new { p.CorpusId, p.Doi })
            .HasFilter("[Doi] IS NOT NULL")
            .IsUnique();

        builder.Entity<Paper>()
            .HasIndex(p => new { p.CorpusId, p.Title, p.Year });

        builder.Entity<PaperSourceRecord>()
            .HasOne(r => r.Paper)
            .WithMany(p => p.SourceRecords)
            .HasForeignKey(r => r.PaperId)
            .OnDelete(DeleteBehavior.Cascade);

        // Restrict, no Cascade: Corpus -> Paper -> PaperSourceRecord ya es una ruta de cascada
        // completa; agregar una segunda via Corpus -> CorpusSourceFile -> PaperSourceRecord
        // generaria "multiple cascade paths" en SQL Server (error 1785). Al borrar un Corpus,
        // sus Paper se cascadean primero (arrastrando los PaperSourceRecord), asi que para
        // cuando se intenta borrar el CorpusSourceFile ya no quedan filas que lo referencien.
        builder.Entity<PaperSourceRecord>()
            .HasOne(r => r.CorpusSourceFile)
            .WithMany()
            .HasForeignKey(r => r.CorpusSourceFileId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
