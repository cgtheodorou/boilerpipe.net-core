namespace Boilerpipe.Net.Estimators {
  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Extractors;

  /// <summary>
  ///   Estimates the "goodness" of a <see cref="IBoilerpipeExtractor" /> on a given document.
  /// </summary>
  public sealed class SimpleEstimator {
    /// <summary>
    ///   Returns the singleton instance of <see cref="SimpleEstimator" />
    /// </summary>
    public static readonly SimpleEstimator Instance = new SimpleEstimator();

    private SimpleEstimator() {
    }

    /// <summary>
    ///   <para>
    ///     Given the statistics of the document before and after applying the <see cref="IBoilerpipeExtractor" />
    ///     can we regard the extraction quality (too) low?
    ///   </para>
    ///   <para>
    ///     Works well with <see cref="DefaultExtractor" />, <see cref="ArticleExtractor" /> and others.
    ///   </para>
    /// </summary>
    /// <param name="dsBefore"></param>
    /// <param name="dsAfter"></param>
    /// <returns>true if low quality is to be expected.</returns>
    public bool IsLowQuality(TextDocumentStatistics dsBefore, TextDocumentStatistics dsAfter) {
      if (dsBefore.NumWords < 90 || dsAfter.NumWords < 70) {
        return true;
      }

      if (dsAfter.AvgNumWords < 25) {
        return true;
      }

      return false;
    }
  }
}
