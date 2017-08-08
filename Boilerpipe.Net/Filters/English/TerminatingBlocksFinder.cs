namespace Boilerpipe.Net.Filters.English {
  using System.Linq;

  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Labels;

  /// <summary>
  ///   Finds blocks which are potentially indicating the end of an article text and
  ///   marks them with <see cref="DefaultLabels.INDICATES_END_OF_TEXT" />. This can be used
  ///   in conjunction with a downstream <see cref="IgnoreBlocksAfterContentFilter" />.
  /// </summary>
  /// <see cref="IgnoreBlocksAfterContentFilter" />
  public class TerminatingBlocksFinder : IBoilerpipeFilter {
    /// <summary>
    ///   The singleton instance for <see cref="TerminatingBlocksFinder" />.
    /// </summary>
    public static readonly TerminatingBlocksFinder Instance = new TerminatingBlocksFinder();

    // public static long timeSpent = 0;

    public bool   Process(TextDocument doc) {
      bool changes = false;

      // long t = System.currentTimeMillis();

      foreach (TextBlock tb in doc.TextBlocks) {
        int numWords = tb.NumWords;
        if (numWords < 15) {
          string text = tb.Text.Trim();
          int len = text.Length;
          if (len >= 8) {
            string textLC = text.ToLower();
            if (textLC.StartsWith("comments") || StartsWithNumber(textLC, len, " comments", " users responded in")
                || textLC.StartsWith("© reuters") || textLC.StartsWith("please rate this")
                || textLC.StartsWith("post a comment") || textLC.Contains("what you think...")
                || textLC.Contains("add your comment") || textLC.Contains("add comment")
                || textLC.Contains("reader views") || textLC.Contains("have your say")
                || textLC.Contains("reader comments") || textLC.Contains("rätta artikeln")
                || textLC.Equals("thanks for your comments - this feedback is now closed")) {
              tb.AddLabel(DefaultLabels.INDICATES_END_OF_TEXT);
              changes = true;
            }
          }
        }
      }

      // timeSpent += System.currentTimeMillis() - t;

      return changes;
    }

    /**
	 * Checks whether the given text t starts with a sequence of digits,
	 * followed by one of the given strings.
	 *
	 * @param t
	 *            The text to examine
	 * @param len
	 *            The length of the text to examine
	 * @param str
	 *            Any strings that may follow the digits.
	 * @return true if at least one combination matches
	 */

    private static bool StartsWithNumber(string t, int len, params string[] str) {
      int j = 0;
      while (j < len && IsDigit(t[j])) {
        j++;
      }
      if (j != 0) {
        //return str.Any(s => t.StartsWith(s, j));
        return str.Any(s => t.Substring(j).StartsWith(s));
      }
      return false;
    }

    private static bool IsDigit(char c) {
      return c >= '0' && c <= '9';
    }
  }
}
