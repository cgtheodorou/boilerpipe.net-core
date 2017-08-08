namespace Boilerpipe.Net {
  using System.IO;

  using Boilerpipe.Net.Document;

  /// <summary>
  ///   Describes a complete filter pipeline.
  /// </summary>
  public interface IBoilerpipeExtractor : IBoilerpipeFilter {
    /// <summary>
    ///   Extracts text from the HTML code given as a string.
    /// </summary>
    /// <param name="html">The HTML code as a string.</param>
    /// <returns>The extracted text.</returns>
    /// <exception cref="BoilerpipeProcessingException"></exception>
    string GetText(string html);
    
    /// <summary>
    ///   text from the HTML code available from the given <see cref="TextReader" />.
    /// </summary>
    /// <param name="reader">The Reader containing the HTML</param>
    /// <returns>The extracted text.</returns>
    /// <exception cref="BoilerpipeProcessingException"></exception>
    string GetText(TextReader reader);

    /// <summary>
    ///   Extracts text from the given <see cref="TextDocument" /> object.
    /// </summary>
    /// <param name="doc">The <see cref="TextDocument" />.</param>
    /// <returns>The extracted text.</returns>
    /// <exception cref="BoilerpipeProcessingException"></exception>
    string GetText(TextDocument doc);
  }
}
