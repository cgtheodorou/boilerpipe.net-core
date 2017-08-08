namespace Boilerpipe.Net.Sax {
  using global::Sax.Net;

  /// <summary>
  ///   Defines an action that is to be performed whenever a particular tag occurs during HTML parsing.
  /// </summary>
  public interface ITagAction {
    bool ChangesTagLevel { get; }

    bool Start(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName, IAttributes attributes);

    bool End(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName);
  }
}
