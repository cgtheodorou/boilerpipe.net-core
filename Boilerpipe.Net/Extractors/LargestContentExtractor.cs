namespace Boilerpipe.Net.Extractors {
  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Filters.English;
  using Boilerpipe.Net.Filters.Heuristics;

  /// <summary>
  ///   A full-text extractor which extracts the largest text component of a page.
  ///   For news articles, it may perform better than the <see cref="DefaultExtractor" />,
  ///   but usually worse than <see cref="ArticleExtractor" />.
  /// </summary>
  public sealed class LargestContentExtractor : BaseExtractor {
    /// <summary>
    ///   The singleton instance for <see cref="LargestContentExtractor" />
    /// </summary>
    public static readonly LargestContentExtractor Instance = new LargestContentExtractor();

    private LargestContentExtractor() {
    }

    public override bool Process(TextDocument doc) {
      return NumWordsRulesClassifier.Instance.Process(doc)
             | BlockProximityFusion.MaxDistance1.Process(doc)
             | KeepLargestBlockFilter.Instance.Process(doc);
    }
  }
}
