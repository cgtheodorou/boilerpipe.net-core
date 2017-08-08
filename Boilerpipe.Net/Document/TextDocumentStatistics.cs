namespace Boilerpipe.Net.Document {
  /// <summary>
  ///   Provides shallow statistics on a given TextDocument
  /// </summary>
  public sealed class TextDocumentStatistics {
    private readonly int _numBlocks;
    private readonly int _numWords;

    /// <summary>
    ///   Computes statistics on a given <see cref="TextDocument" />.
    /// </summary>
    /// <param name="doc">The <see cref="TextDocument" />.</param>
    /// <param name="contentOnly">if true then only content is counted</param>
    public TextDocumentStatistics(TextDocument doc, bool contentOnly) {
      foreach (TextBlock tb in doc.TextBlocks) {
        if (contentOnly && !tb.IsContent) {
          continue;
        }

        _numWords += tb.NumWords;
        _numBlocks++;
      }
    }

    /// <summary>
    ///   Returns the average number of words at block-level (= overall number of words divided by the number of blocks).
    /// </summary>
    public float AvgNumWords {
      get {
        return _numWords / (float)_numBlocks;
      }
    }

    /// <summary>
    ///   Returns the overall number of words in all blocks.
    /// </summary>
    public int NumWords {
      get {
        return _numWords;
      }
    }
  }
}
