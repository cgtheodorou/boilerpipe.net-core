using Boilerpipe.Net.Document;

namespace Boilerpipe.Net {
  /// <summary>
  /// A source that returns <see cref="GetTextDocument"/>s
  /// </summary>
  public interface IBoilerpipeInput {
    /// <summary>
    /// Returns (somehow) a <see cref="GetTextDocument"/>.
    /// </summary>
    /// <returns>A <see cref="GetTextDocument"/></returns>
    /// <exception cref="BoilerpipeProcessingException"></exception>
    TextDocument GetTextDocument();
  }
}
