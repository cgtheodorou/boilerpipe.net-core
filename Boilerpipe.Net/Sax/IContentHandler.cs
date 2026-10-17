namespace Boilerpipe.Net.Sax {
  /// <summary>
  ///   Receives notification of the logical content of a parsed HTML document.
  /// </summary>
  public interface IContentHandler {
    void StartDocument();

    void EndDocument();

    void StartElement(string uri, string localName, string qName, IAttributes attributes);

    void EndElement(string uri, string localName, string qName);

    void Characters(char[] characters, int start, int length);
  }
}
