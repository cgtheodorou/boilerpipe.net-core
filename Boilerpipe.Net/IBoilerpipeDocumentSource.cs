namespace Boilerpipe.Net {
  using Boilerpipe.Net.Document;

  /// <summary>
  ///   Something that can be represented as a <see cref="TextDocument" />.
  /// </summary>
  public interface IBoilerpipeDocumentSource {
    /// <summary>
    ///   Returns a <see cref="TextDocument" />.
    /// </summary>
    /// <returns>The <see cref="TextDocument" /></returns>
    TextDocument ToTextDocument();
  }
}
