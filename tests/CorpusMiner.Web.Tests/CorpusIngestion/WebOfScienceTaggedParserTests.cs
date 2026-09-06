using System.Text;
using CorpusMiner.Web.Services.CorpusIngestion;

namespace CorpusMiner.Web.Tests.CorpusIngestion;

public class WebOfScienceTaggedParserTests
{
    private const string SampleExport = """
        FN Clarivate Analytics Web of Science
        VR 1.0
        PT J
        AU Smith, J.
           Doe, A.
        TI A Study of Something
           Interesting
        SO Journal of Testing
        PY 2020
        DI 10.1000/abc123
        AB This is an abstract.
           It continues here.
        ER

        PT J
        AU Lee, K.
        TI Another Paper
        SO Journal of Testing
        PY 2021
        ER

        EF
        """;

    [Fact]
    public void Parse_Returns_One_Record_Per_ER()
    {
        var records = new WebOfScienceTaggedParser().Parse(ToStream(SampleExport));

        Assert.Equal(2, records.Count);
    }

    [Fact]
    public void Parse_Joins_Continuation_Lines_Into_The_Previous_Field()
    {
        var records = new WebOfScienceTaggedParser().Parse(ToStream(SampleExport));

        var first = records[0];
        Assert.Equal("A Study of Something Interesting", first.Title);
        Assert.Equal("This is an abstract. It continues here.", first.Abstract);
    }

    [Fact]
    public void Parse_Extracts_Doi_Year_And_SourceTitle()
    {
        var records = new WebOfScienceTaggedParser().Parse(ToStream(SampleExport));

        var first = records[0];
        Assert.Equal("10.1000/abc123", first.Doi);
        Assert.Equal(2020, first.Year);
        Assert.Equal("Journal of Testing", first.SourceTitle);
    }

    [Fact]
    public void Parse_Handles_Record_Without_Doi()
    {
        var records = new WebOfScienceTaggedParser().Parse(ToStream(SampleExport));

        var second = records[1];
        Assert.Equal("Another Paper", second.Title);
        Assert.Null(second.Doi);
        Assert.Equal(2021, second.Year);
    }

    private static MemoryStream ToStream(string text) => new(Encoding.UTF8.GetBytes(text));
}
