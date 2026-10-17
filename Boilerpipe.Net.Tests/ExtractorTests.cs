namespace Boilerpipe.Net.Tests {
  using System;
  using System.Globalization;
  using System.IO;
  using System.Text;

  using Boilerpipe.Net.Extractors;
  using Boilerpipe.Net.Sax;

  using Xunit;

  public class ExtractorTests {
    private static readonly string FixturesDir = Path.Combine(AppContext.BaseDirectory, "Fixtures");

    public static TheoryData<string, string> Cases() {
      var data = new TheoryData<string, string>();
      foreach (string fixture in new[] { "news-article", "blog-post" }) {
        foreach (string extractor in new[] { "article", "sentences", "canola", "default", "everything", "mink", "largest", "numwords" }) {
          data.Add(fixture, extractor);
        }
      }
      return data;
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public void GetText_MatchesExpectedOutput(string fixture, string extractor) {
      Assert.Equal(ReadExpected(fixture, extractor), CreateExtractor(extractor).GetText(ReadFixture(fixture)));
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public void GetText_IsIndependentOfCurrentCulture(string fixture, string extractor) {
      CultureInfo original = CultureInfo.CurrentCulture;
      try {
        CultureInfo.CurrentCulture = new CultureInfo("tr-TR");
        Assert.Equal(ReadExpected(fixture, extractor), CreateExtractor(extractor).GetText(ReadFixture(fixture)));
      } finally {
        CultureInfo.CurrentCulture = original;
      }
    }

    [Fact]
    public void GetText_FromReader_MatchesString() {
      string html = ReadFixture("news-article");
      Assert.Equal(ArticleExtractor.Instance.GetText(html), ArticleExtractor.Instance.GetText(new StringReader(html)));
    }

    [Fact]
    public void GetText_FromStreamWithEncoding_MatchesString() {
      using var stream = File.OpenRead(FixturePath("news-article"));
      string text = ArticleExtractor.Instance.GetText(new InputSource(stream) { Encoding = Encoding.UTF8 });
      Assert.Equal(ReadExpected("news-article", "article"), text);
    }

    [Fact]
    public void GetText_FromStreamWithoutEncoding_DetectsEncoding() {
      using var stream = File.OpenRead(FixturePath("news-article"));
      Assert.Equal(ReadExpected("news-article", "article"), ArticleExtractor.Instance.GetText(new InputSource(stream)));
    }

    [Fact]
    public void GetText_FromFilePath_MatchesString() {
      Assert.Equal(ReadExpected("news-article", "article"), ArticleExtractor.Instance.GetText(new InputSource(FixturePath("news-article"))));
    }

    [Theory]
    [InlineData("news-article", "City Council Approves New Riverside Park | The Daily Ledger")]
    [InlineData("blog-post", "Notes on Sourdough Hydration - Flour and Water")]
    public void GetTextDocument_ExtractsTitle(string fixture, string expectedTitle) {
      var input = new BoilerpipeSAXInput(new InputSource(new StringReader(ReadFixture(fixture))));
      Assert.Equal(expectedTitle, input.GetTextDocument().Title);
    }

    [Fact]
    public void GetText_InlineElements_StayInOneBlock() {
      const string html = "<html><body><p>Read <a href=\"/a\">the full report</a> or <b>skip</b> <i>ahead</i>.</p></body></html>";
      Assert.Equal("Read the full report or skip ahead.\n", KeepEverythingExtractor.Instance.GetText(html));
    }

    [Fact]
    public void GetText_DecodesEntitiesOnce() {
      const string html = "<html><head><title>Fish &amp; Chips &#8211; &lt;Menu&gt;</title></head>"
        + "<body><p>Caf&eacute;&nbsp;prices &#x2192; write &amp;lt; for &lt;</p></body></html>";
      var input = new BoilerpipeSAXInput(new InputSource(new StringReader(html)));
      Assert.Equal("Fish & Chips – <Menu>", input.GetTextDocument().Title);
      Assert.Equal("Café prices → write &lt; for <\n", KeepEverythingExtractor.Instance.GetText(html));
    }

    [Fact]
    public void GetText_EmptyHtml_ReturnsEmpty() {
      Assert.Equal(string.Empty, CommonExtractors.ArticleExtractor.GetText(string.Empty));
    }

    [Fact]
    public void GetText_MissingFile_ThrowsBoilerpipeProcessingException() {
      var source = new InputSource(Path.Combine(FixturesDir, "does-not-exist.html"));
      Assert.Throws<BoilerpipeProcessingException>(() => ArticleExtractor.Instance.GetText(source));
    }

    private static BaseExtractor CreateExtractor(string name) => name switch {
      "article" => ArticleExtractor.Instance,
      "sentences" => ArticleSentencesExtractor.Instance,
      "canola" => CanolaExtractor.Instance,
      "default" => DefaultExtractor.Instance,
      "everything" => KeepEverythingExtractor.Instance,
      "mink" => new KeepEverythingWithMinKWordsExtractor(5),
      "largest" => LargestContentExtractor.Instance,
      "numwords" => NumWordsRulesExtractor.Instance,
      _ => throw new ArgumentOutOfRangeException(nameof(name), name, null),
    };

    private static string FixturePath(string fixture) => Path.Combine(FixturesDir, fixture + ".html");

    private static string ReadFixture(string fixture) => File.ReadAllText(FixturePath(fixture));

    private static string ReadExpected(string fixture, string extractor) =>
      File.ReadAllText(Path.Combine(FixturesDir, "Expected", $"{fixture}.{extractor}.txt"));
  }
}
