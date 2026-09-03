using System.Text;
using CorpusMiner.Web.Data;
using CorpusMiner.Web.Services.CorpusIngestion;
using Microsoft.EntityFrameworkCore;

namespace CorpusMiner.Web.Tests.CorpusIngestion;

public class CorpusIngestionServiceTests
{
    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    private static async Task<(Corpus Corpus, CorpusSourceFile SourceFile)> SeedCorpusAsync(
        ApplicationDbContext db, CorpusSourceType sourceType, string fileName)
    {
        var research = new MiningResearch { Title = "Test research", CreatedByUserId = "user-1" };
        db.MiningResearches.Add(research);
        var corpus = new Corpus { MiningResearch = research, Title = "Test corpus" };
        db.Corpora.Add(corpus);
        await db.SaveChangesAsync();

        var sourceFile = new CorpusSourceFile
        {
            CorpusId = corpus.Id,
            OriginalFileName = fileName,
            SourceType = sourceType,
            BlobPath = $"test/{fileName}",
            UploadedByUserId = "user-1",
        };
        db.CorpusSourceFiles.Add(sourceFile);
        await db.SaveChangesAsync();

        return (corpus, sourceFile);
    }

    private static MemoryStream ToStream(string text) => new(Encoding.UTF8.GetBytes(text));

    [Fact]
    public async Task Ingest_Merges_Duplicate_Doi_Within_The_Same_File_Into_One_Paper()
    {
        await using var db = CreateDbContext();
        var (corpus, sourceFile) = await SeedCorpusAsync(db, CorpusSourceType.WebOfScience, "wos.txt");
        var service = new CorpusIngestionService(db, [new WebOfScienceTaggedParser(), new ScopusCsvParser()]);

        const string content = """
            PT J
            TI First Copy
            PY 2020
            DI 10.1/same
            ER

            PT J
            TI Second Copy
            PY 2020
            DI 10.1/same
            ER

            EF
            """;

        await service.IngestAsync(sourceFile, ToStream(content));

        var papers = await db.Papers.Where(p => p.CorpusId == corpus.Id).ToListAsync();
        Assert.Single(papers);

        var sourceRecords = await db.PaperSourceRecords.Where(r => r.CorpusSourceFileId == sourceFile.Id).ToListAsync();
        Assert.Equal(2, sourceRecords.Count);
        Assert.Equal(CorpusSourceFileStatus.Parsed, sourceFile.Status);
        Assert.Equal(2, sourceFile.RecordCount);
    }

    [Fact]
    public async Task Ingest_Creates_Separate_Papers_For_Distinct_Dois()
    {
        await using var db = CreateDbContext();
        var (corpus, sourceFile) = await SeedCorpusAsync(db, CorpusSourceType.WebOfScience, "wos.txt");
        var service = new CorpusIngestionService(db, [new WebOfScienceTaggedParser(), new ScopusCsvParser()]);

        const string content = """
            PT J
            TI Paper One
            PY 2020
            DI 10.1/one
            ER

            PT J
            TI Paper Two
            PY 2021
            DI 10.1/two
            ER

            EF
            """;

        await service.IngestAsync(sourceFile, ToStream(content));

        var papers = await db.Papers.Where(p => p.CorpusId == corpus.Id).ToListAsync();
        Assert.Equal(2, papers.Count);
    }

    [Fact]
    public async Task Ingest_Merges_Across_Sources_By_Title_And_Year_When_No_Doi_Overlaps()
    {
        await using var db = CreateDbContext();
        var (corpus, wosFile) = await SeedCorpusAsync(db, CorpusSourceType.WebOfScience, "wos.txt");

        var service = new CorpusIngestionService(db, [new WebOfScienceTaggedParser(), new ScopusCsvParser()]);

        const string wosContent = """
            PT J
            TI Shared Paper
            PY 2020
            ER

            EF
            """;
        await service.IngestAsync(wosFile, ToStream(wosContent));

        var scopusFile = new CorpusSourceFile
        {
            CorpusId = corpus.Id,
            OriginalFileName = "scopus.csv",
            SourceType = CorpusSourceType.Scopus,
            BlobPath = "test/scopus.csv",
            UploadedByUserId = "user-1",
        };
        db.CorpusSourceFiles.Add(scopusFile);
        await db.SaveChangesAsync();

        const string scopusContent = """
            Authors,Title,Year,Source title,DOI,Abstract
            "Someone",Shared Paper,2020,Some Journal,,
            """;
        await service.IngestAsync(scopusFile, ToStream(scopusContent));

        var papers = await db.Papers.Where(p => p.CorpusId == corpus.Id).ToListAsync();
        Assert.Single(papers);
        Assert.Contains("WebOfScience", papers[0].RawMetadataJson);
        Assert.Contains("Scopus", papers[0].RawMetadataJson);
    }
}
