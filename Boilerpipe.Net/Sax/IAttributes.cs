namespace Boilerpipe.Net.Sax {
  /// <summary>
  ///   The attributes of an HTML element, as reported to <see cref="IContentHandler.StartElement" />.
  /// </summary>
  public interface IAttributes {
    int Length { get; }

    string GetLocalName(int index);

    string GetQName(int index);

    string GetValue(int index);

    /// <returns>The index of the attribute, or -1 if it is not present.</returns>
    int GetIndex(string qName);

    /// <returns>The attribute value, or <c>null</c> if it is not present.</returns>
    string GetValue(string qName);
  }
}
