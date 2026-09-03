using System.Text;
using CorpusMiner.Web.Services.CorpusIngestion;

namespace CorpusMiner.Web.Tests.CorpusIngestion;

public class ScopusCsvParserTests
{
    private const string SampleExport = """
        Authors,Title,Year,Source title,DOI,Abstract
        "Smith, J.",A Study of Something,2020,Journal of Testing,10.1000/abc123,"This is an abstract."
        "Lee, K.",Another Paper,2021,Journal of Testing,,
        """;

    [Fact]
    public void Parse_Returns_One_Record_Per_Row()
    {
        var records = new ScopusCsvParser().Parse(ToStream(SampleExport));

        Assert.Equal(2, records.Count);
    }

    [Fact]
    public void Parse_Extracts_Known_Columns_And_Keeps_Raw_Fields()
    {
        var records = new ScopusCsvParser().Parse(ToStream(SampleExport));

        var first = records[0];
        Assert.Equal("A Study of Something", first.Title);
        Assert.Equal(2020, first.Year);
        Assert.Equal("Journal of Testing", first.SourceTitle);
        Assert.Equal("10.1000/abc123", first.Doi);
        Assert.Equal("This is an abstract.", first.Abstract);
        Assert.Equal("Smith, J.", first.RawFields["Authors"]);
    }

    [Fact]
    public void Parse_Handles_Missing_Doi_As_Null()
    {
        var records = new ScopusCsvParser().Parse(ToStream(SampleExport));

        Assert.Null(records[1].Doi);
    }

    private static MemoryStream ToStream(string text) => new(Encoding.UTF8.GetBytes(text));
}
