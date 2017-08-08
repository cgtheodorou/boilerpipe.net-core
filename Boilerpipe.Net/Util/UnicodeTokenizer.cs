namespace Boilerpipe.Net.Util {
  using System.Text;
  using System.Text.RegularExpressions;

  /// <summary>
  ///   Tokenizes text according to Unicode word boundaries and strips off non-word characters.
  /// </summary>
  public class UnicodeTokenizer {
    private static readonly Regex PatWordBoundary = new Regex("\\b", RegexOptions.Compiled);

    private static readonly Regex PatNotWordBoundary =
      new Regex("[\u2063]*([\\\"'\\.,\\!\\@\\-\\:\\;\\$\\?\\(\\)/])[\u2063]*", RegexOptions.Compiled);

    /// <summary>
    ///   Tokenizes the text and returns an array of tokens.
    /// </summary>
    /// <param name="text">The text</param>
    /// <returns>The tokens</returns>
    public static string[] Tokenize(StringBuilder text) {
      return Regex.Split(
               Regex.Replace(
                 PatNotWordBoundary.Replace(PatWordBoundary.Replace(text.ToString(), "\u2063"), "$1"), "[ \u2063]+", " "
               ).Trim(), "[ ]+"
             );
    }
  }
}
