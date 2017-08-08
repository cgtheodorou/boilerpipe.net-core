namespace Boilerpipe.Net.Sax {
  using System;
  using System.Text.RegularExpressions;

  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Labels;

  using global::Sax.Net;

  /// <summary>
  ///   Defines an action that is to be performed whenever a particular tag occurs during HTML parsing.
  /// </summary>
  public abstract class CommonTagActions {
    /// <summary>
    ///   Marks this tag as "ignorable", i.e. all its inner content is silently skipped.
    /// </summary>
    public static readonly ITagAction IgnorableElement = new IgnoreableElementTagAction();

    /// <summary>
    ///   Marks this tag as "anchor" (this should usually only be set for the <code>&lt;A&gt;</code> tag).
    ///   Anchor tags may not be nested.
    /// </summary>
    public static readonly ITagAction AnchorText = new AnchorTextTagAction();

    /// <summary>
    ///   Marks this tag the body element (this should usually only be set for the <code>&lt;BODY&gt;</code> tag).
    /// </summary>
    public static readonly ITagAction Body = new TaBodyTagAction();

    /// <summary>
    ///   Marks this tag a simple "inline" element, which generates whitespace, but no new block.
    /// </summary>
    public static readonly ITagAction InlineWhitespace = new InlineWhitespaceTagAction();

    /// <summary>
    ///   Marks this tag a simple "inline" element, which neither generates whitespace, nor a new block.
    /// </summary>
    public static readonly ITagAction InlineNoWhitespace = new InlineNoWhitespaceTagAction();

    /// <summary>
    ///   Explicitly marks this tag a simple "block-level" element, which always generates whitespace
    /// </summary>
    public static readonly ITagAction BlockLevel = new BlockLevelTagAction();

    /// <summary>
    ///   Special TagAction for the <code>&lt;FONT&gt;</code> tag, which keeps track of the
    ///   absolute and relative font size.
    /// </summary>
    public static readonly ITagAction Font = new FontTagAction();

    private static readonly Regex FontSizeRegex = new Regex("([\\+\\-]?)([0-9])", RegexOptions.Compiled);

    private CommonTagActions() {
    }

    private class AnchorTextTagAction : ITagAction {
      public bool Start(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName, IAttributes attributes) {
        if (instance.InAnchor++ > 0) {
          // as nested A elements are not allowed per specification, we
          // are probably reaching this branch due to a bug in the XML
          // parser
          Console.Error.WriteLine(
            "Warning: Input contains nested A elements -- You have probably hit a bug in your HTML parser. Please clean the HTML externally and feed it to boilerpipe again. Trying to recover somehow...");

          End(instance, uri, localName, qName);
        }
        if (instance.InIgnorableElement == 0) {
          instance.AddWhitespaceIfNecessary();
          instance.TokenBuffer.Append(BoilerpipeHtmlContentHandler.ANCHOR_TEXT_START);
          instance.TokenBuffer.Append(' ');
          instance.SbLastWasWhitespace = true;
        }
        return false;
      }

      public bool End(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName) {
        if (--instance.InAnchor == 0) {
          if (instance.InIgnorableElement == 0) {
            instance.AddWhitespaceIfNecessary();
            instance.TokenBuffer.Append(BoilerpipeHtmlContentHandler.ANCHOR_TEXT_END);
            instance.TokenBuffer.Append(' ');
            instance.SbLastWasWhitespace = true;
          }
        }
        return false;
      }

      public bool ChangesTagLevel {
        get {
          return true;
        }
      }
    };

    private class BlockLevelTagAction : ITagAction {
      public bool Start(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName, IAttributes attributes) {
        return true;
      }

      public bool End(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName) {
        return true;
      }

      public bool ChangesTagLevel {
        get {
          return true;
        }
      }
    }

    /// <summary>
    ///   <see cref="CommonTagActionst" /> for block-level elements, which triggers some <see cref="LabelAction" /> on the
    ///   generated <see cref="TextBlock" />.
    /// </summary>
    public sealed class BlockTagLabelAction : ITagAction {
      private readonly LabelAction _action;

      public BlockTagLabelAction(LabelAction action) {
        _action = action;
      }

      public bool Start(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName, IAttributes attributes) {
        instance.AddLabelAction(_action);
        return true;
      }

      public bool End(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName) {
        return true;
      }

      public bool ChangesTagLevel {
        get {
          return true;
        }
      }
    }

    public sealed class Chained : ITagAction {
      private readonly ITagAction t1;
      private readonly ITagAction t2;

      public Chained(ITagAction t1, ITagAction t2) {
        this.t1 = t1;
        this.t2 = t2;
      }

      public bool Start(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName, IAttributes attributes) {
        return t1.Start(instance, uri, localName, qName, attributes) | t2.Start(instance, uri, localName, qName, attributes);
      }

      public bool End(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName) {
        return t1.End(instance, uri, localName, qName) | t2.End(instance, uri, localName, qName);
      }

      public bool ChangesTagLevel {
        get {
          return t1.ChangesTagLevel || t2.ChangesTagLevel;
        }
      }
    }

    private class FontTagAction : ITagAction {
      public bool Start(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName, IAttributes attributes) {
        string sizeAttr = attributes. GetValue("size");
        if (sizeAttr != null) {
          Match m = FontSizeRegex.Match(sizeAttr);
          if (m.Success) {
            string rel = m.Groups[1].Value;
            int val = int.Parse(m.Groups[2].Value);
            int size;
            if (rel.Length == 0) {
              // absolute
              size = val;
            } else {
              // relative
              int prevSize;
              if (instance.FontSizeStack.Count == 0) {
                prevSize = 3;
              } else {
                prevSize = 3;
                foreach (var s in instance.FontSizeStack) {
                  if (s != null) {
                    prevSize = s.Value;
                    break;
                  }
                }
              }
              if (rel[0] == '+') {
                size = prevSize + val;
              } else {
                size = prevSize - val;
              }
            }
            instance.FontSizeStack.AddFirst(size);
          } else {
            instance.FontSizeStack.AddFirst((int?)null);
          }
        } else {
          instance.FontSizeStack.AddFirst((int?)null);
        }
        return false;
      }

      public bool End(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName) {
        instance.FontSizeStack.RemoveFirst();
        return false;
      }

      public bool ChangesTagLevel {
        get {
          return false;
        }
      }
    };

    private class IgnoreableElementTagAction : ITagAction {
      public bool Start(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName, IAttributes attributes) {
        instance.InIgnorableElement++;
        return true;
      }

      public bool End(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName) {
        instance.InIgnorableElement--;
        return true;
      }

      public bool ChangesTagLevel {
        get {
          return true;
        }
      }
    }

    private class InlineNoWhitespaceTagAction : ITagAction {
      public bool Start(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName, IAttributes attributes) {
        return false;
      }

      public bool End(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName) {
        return false;
      }

      public bool ChangesTagLevel {
        get {
          return true;
        }
      }
    }

    /// <summary>
    ///   <see cref="CommonTagActions" /> for inline elements, which triggers some <see cref="LabelAction" /> on the generated
    ///   <see cref="TextBlock" />.
    /// </summary>
    public sealed class InlineTagLabelAction : ITagAction {
      private readonly LabelAction _action;

      public InlineTagLabelAction(LabelAction action) {
        _action = action;
      }

      public bool Start(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName, IAttributes attributes) {
        instance.AddWhitespaceIfNecessary();
        instance.AddLabelAction(_action);
        return false;
      }

      public bool End(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName) {
        instance.AddWhitespaceIfNecessary();
        return false;
      }

      public bool ChangesTagLevel {
        get {
          return false;
        }
      }
    }

    private class InlineWhitespaceTagAction : ITagAction {
      public bool Start(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName, IAttributes attributes) {
        instance.AddWhitespaceIfNecessary();
        return false;
      }

      public bool End(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName) {
        instance.AddWhitespaceIfNecessary();
        return false;
      }

      public bool ChangesTagLevel {
        get {
          return true;
        }
      }
    }

    private class TaBodyTagAction : ITagAction {
      public bool Start(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName, IAttributes attributes) {
        instance.FlushBlock();
        instance.InBody++;
        return false;
      }

      public bool End(BoilerpipeHtmlContentHandler instance, string uri, string localName, string qName) {
        instance.FlushBlock();
        instance.InBody--;
        return false;
      }

      public bool ChangesTagLevel {
        get {
          return true;
        }
      }
    }
  }
}
