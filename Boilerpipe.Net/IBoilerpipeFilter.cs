namespace Boilerpipe.Net {
  using Boilerpipe.Net.Document;

  /// <summary>
  ///   A generic <see cref="IBoilerpipeFilter" />.
  ///   Takes a <see cref="TextDocument" /> and processes it somehow.
  /// </summary>
  public interface IBoilerpipeFilter {
    /// <summary>
    ///   Processes the given document <code>doc</code>.
    /// </summary>
    /// <param name="doc">The <see cref="TextDocument" /> that is to be processed.</param>
    /// <returns><code>true</code> if changes have been made to the <see cref="TextDocument" />.</returns>
    /// <exception cref="BoilerpipeProcessingException"></exception>
    bool Process(TextDocument doc);
  }
}
