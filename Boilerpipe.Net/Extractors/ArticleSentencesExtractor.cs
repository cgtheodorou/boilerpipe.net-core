namespace Boilerpipe.Net.Extractors {
  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Filters.Simple;

  /// <summary>
  ///   A full-text extractor which is tuned towards extracting sentences from news articles.
  /// </summary>
  public sealed class ArticleSentencesExtractor : BaseExtractor {
    /// <summary>
    ///   The singleton instance for <see cref="ArticleSentencesExtractor" />
    /// </summary>
    public static readonly ArticleSentencesExtractor Instance = new ArticleSentencesExtractor();

    public override bool Process(TextDocument doc) {
      return ArticleExtractor.Instance.Process(doc)
             | SplitParagraphBlocksFilter.Instance.Process(doc)
             | MinClauseWordsFilter.Instance.Process(doc);
    }
  }
}
