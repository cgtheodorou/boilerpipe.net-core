namespace Boilerpipe.Net.Extractors {
  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Filters.English;

  /// <summary>
  ///   A quite generic full-text extractor solely based upon the number of words per
  ///   block (the current, the previous and the next block).
  /// </summary>
  public class NumWordsRulesExtractor : BaseExtractor {
    /// <summary>
    ///   The singleton instance for <see cref="NumWordsRulesExtractor" />
    /// </summary>
    public static readonly NumWordsRulesExtractor Instance = new NumWordsRulesExtractor();

    public override bool Process(TextDocument doc) {
      return NumWordsRulesClassifier.Instance.Process(doc);
    }
  }
}
