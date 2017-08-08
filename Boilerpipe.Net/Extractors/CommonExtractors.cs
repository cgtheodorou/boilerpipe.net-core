namespace Boilerpipe.Net.Extractors {
  /// <summary>
  /// Provides quick access to common <see cref="IBoilerpipeExtractor"/>s.
  /// </summary>
  public static class CommonExtractors {
    /// <summary>
    ///   Works very well for most types of Article-like HTML.
    /// </summary>
    public static readonly ArticleExtractor ArticleExtractor = ArticleExtractor.Instance;

    /// <summary>
    ///   Usually worse than <see cref="ArticleExtractor" />, but simpler/no heuristics.
    /// </summary>
    public static readonly DefaultExtractor DefaultExtractor = DefaultExtractor.Instance;

    /// <summary>
    ///   Like <see cref="DefaultExtractor" />, but keeps the largest text block only.
    /// </summary>
    public static readonly LargestContentExtractor LargestContentExtractor = LargestContentExtractor.Instance;

    /// <summary>
    ///   Trained on krdwrd Canola (different definition of "boilerplate"). You may give it a try.
    /// </summary>
    public static readonly CanolaExtractor CanolaExtractor = CanolaExtractor.Instance;

    /// <summary>
    /// Dummy Extractor; should return the input text. Use this to double-check
	  /// that your problem is within a particular <see cref="IBoilerpipeExtractor"/>, or
	  /// somewhere else.
    /// </summary>
    public static readonly KeepEverythingExtractor KeepEverythingExtractor = KeepEverythingExtractor.Instance;
  }
}
