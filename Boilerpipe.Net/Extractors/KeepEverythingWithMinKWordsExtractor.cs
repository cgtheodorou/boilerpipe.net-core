namespace Boilerpipe.Net.Extractors {
  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Filters.Heuristics;
  using Boilerpipe.Net.Filters.Simple;

  /// <summary>
  ///   A full-text extractor which extracts the largest text component of a page.
  ///   For news articles, it may perform better than the <see cref="DefaultExtractor" />,
  ///   but usually worse than <see cref="ArticleExtractor" />.
  /// </summary>
  public sealed class KeepEverythingWithMinKWordsExtractor : BaseExtractor {
    private readonly MinWordsFilter _filter;

    /// <summary>
    ///   Creates a new instance of <see cref="KeepEverythingWithMinKWordsExtractor" />
    /// </summary>
    /// <param name="kMin"></param>
    public KeepEverythingWithMinKWordsExtractor(int kMin) {
      _filter = new MinWordsFilter(kMin);
    }

    public override bool Process(TextDocument doc) {
      return SimpleBlockFusionProcessor.Instance.Process(doc)
             | MarkEverythingContentFilter.Instance.Process(doc)
             | _filter.Process(doc);
    }
  }
}
