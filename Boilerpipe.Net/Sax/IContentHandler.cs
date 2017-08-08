//namespace Boilerpipe.Net.Parser {
//  using System.Collections.Generic;

//  using Boilerpipe.Net.Document;

//  /// <summary>
//  ///   Receive notification of the logical content of a document.
//  /// </summary>
//  public interface IContentHandler {
//    /// <summary>
//    ///   Receive notification of the beginning of a document.
//    /// </summary>
//    void StartDocument();

//    /// <summary>
//    ///   Receive notification of the end of a document.
//    /// </summary>
//    void EndDocument();

//    /// <summary>
//    ///   Receive notification of the beginning of an element.
//    /// </summary>
//    /// <param name="uri">The Namespace URI, or empty string</param>
//    /// <param name="localName">The local name (without prefix), or empty string</param>
//    /// <param name="qName">The qualified (prefixed) name, or empty string</param>
//    /// <param name="attributes">The attributes for the element</param>
//    void StartElement(string uri, string localName, string qName, IDictionary<string, string> attributes);

//    /// <summary>
//    ///   Receive notification of the end of an element.
//    /// </summary>
//    /// <param name="uri">The Namespace URI, or empty string</param>
//    /// <param name="localName">The local name (without prefix), or empty string</param>
//    /// <param name="qName">The qualified (prefixed) name, or empty string</param>
//    void EndElement(string uri, string localName, string qName);

//    /// <summary>
//    ///   Receive notification of character data.
//    /// </summary>
//    /// <param name="characters">The caracters from the document.</param>
//    /// <param name="start">Start position in the array</param>
//    /// <param name="length">The number of characters to read from the array</param>
//    void Characters(char[] characters, int start, int length);

//    /// <summary>
//    ///   Returns a <see cref="TextDocument" /> containing the extracted <see cref="TextBlock" />s.
//    /// </summary>
//    /// <returns>The <see cref="TextDocument" /></returns>
//    /// <remarks>
//    ///   NOTE: Only call this after parsing.
//    /// </remarks>
//    TextDocument ToTextDocument();
//  }
//}
