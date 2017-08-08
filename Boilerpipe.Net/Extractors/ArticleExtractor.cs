namespace Boilerpipe.Net.Extractors {
  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Filters.English;
  using Boilerpipe.Net.Filters.Heuristics;
  using Boilerpipe.Net.Filters.Simple;

  /// <summary>
  ///   A full-text extractor which is tuned towards news articles. In this scenario
  ///   it achieves higher accuracy than <see cref="DefaultExtractor" />.
  /// </summary>
  public class ArticleExtractor : BaseExtractor {
    /// <summary>
    ///   The singleton instance for <see cref="ArticleExtractor" />
    /// </summary>
    public static readonly ArticleExtractor Instance = new ArticleExtractor();

    public override bool Process(TextDocument doc) {
      return TerminatingBlocksFinder.Instance.Process(doc)
             | new DocumentTitleMatchClassifier(doc.Title).Process(doc)
             | NumWordsRulesClassifier.Instance.Process(doc)
             | IgnoreBlocksAfterContentFilter.DefaultInstance.Process(doc)
             | BlockProximityFusion.MaxDistance1.Process(doc)
             | BoilerplateBlockFilter.Instance.Process(doc)
             | BlockProximityFusion.MaxDistance1ContentOnly.Process(doc)
             | KeepLargestBlockFilter.Instance.Process(doc)
             | ExpandTitleToContentFilter.Instance.Process(doc);
    }
  }
}
