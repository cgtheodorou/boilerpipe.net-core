namespace Boilerpipe.Net.Sax {
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Text;
  using System.Text.RegularExpressions;

  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Labels;
  using Boilerpipe.Net.Util;

  using global::Sax.Net;
  using global::Sax.Net.Helpers;

  /// <summary>
  ///   A simple ContentHandler used by <see cref="BoilerpipeHtmlParser" />.
  /// </summary>
  public class BoilerpipeHtmlContentHandler : DefaultHandler {
    public const string ANCHOR_TEXT_START = "$\ue00a<";
    public const string ANCHOR_TEXT_END = ">\ue00a$";

    private static readonly Regex PatValidWordCharacter = new Regex("[\\p{L}\\p{Nd}\\p{Nl}\\p{No}]", RegexOptions.Compiled);

    private readonly LinkedList<LinkedList<LabelAction>> _labelStacks = new LinkedList<LinkedList<LabelAction>>();
    private readonly Dictionary<string, ITagAction> _tagActions;
    private readonly List<TextBlock> _textBlocks = new List<TextBlock>();
    private readonly StringBuilder _textBuffer = new StringBuilder();

    internal LinkedList<int?> FontSizeStack = new LinkedList<int?>();
    internal int InAnchor = 0;
    internal int InBody = 0;
    internal int InIgnorableElement = 0;
    internal bool SbLastWasWhitespace = false;
    internal StringBuilder TokenBuffer = new StringBuilder();

    private int _blockTagLevel = -1;
    private BitArray _currentContainedTextElements = new BitArray(short.MaxValue);
    private bool _flush;
    private bool _inAnchorText;
    private string _lastEndTag;
    private Event _lastEvent = Event.None;
    private string _lastStartTag;
    private int _offsetBlocks;
    private int _tagLevel;
    private int _textElementIdx;
    private string _title;

    /// <summary>
    ///   Constructs a <see cref="BoilerpipeHtmlContentHandler" /> using the
    ///   <see cref="DefaultTagActionDictionary" />.
    /// </summary>
    public BoilerpipeHtmlContentHandler()
      : this(DefaultTagActionDictionary.Instance) {
    }

    /// <summary>
    ///   Constructs a <see cref="BoilerpipeHtmlContentHandler" /> using the given
    ///   <see cref="TagActionDictionary" />.
    /// </summary>
    /// <param name="tagActions">
    ///   The <see cref="TagActionDictionary" /> to use, e.g. <see cref="DefaultTagActionDictionary" />.
    /// </param>
    public BoilerpipeHtmlContentHandler(TagActionDictionary tagActions) {
      _tagActions = tagActions;
    }

    public List<TextBlock> TextBlocks {
      get {
        return _textBlocks;
      }
    }

    public string Title {
      get {
        return _title;
      }
      set {
        if (string.IsNullOrEmpty(value)) {
          return;
        }
        _title = value;
      }
    }

    public override void EndDocument() {
      FlushBlock();
    }

    public override void StartDocument() {
    }

    public override void StartElement(string uri, string localName, string qName, IAttributes attributes) {
      _labelStacks.AddLast((LinkedList<LabelAction>)null);

      ITagAction ta;
      if (_tagActions.TryGetValue(localName, out ta)) {
        if (ta.ChangesTagLevel) {
          _tagLevel++;
        }
        _flush = ta.Start(this, uri, localName, qName, attributes) | _flush;
      } else {
        _tagLevel++;
        _flush = true;
      }

      _lastEvent = Event.StartTag;
      _lastStartTag = localName;
    }

    public override void EndElement(string uri, string localName, string qName) {
      ITagAction ta;
      if (_tagActions.TryGetValue(localName, out ta)) {
        _flush = ta.End(this, uri, localName, qName) | _flush;
      } else {
        _flush = true;
      }

      if (ta == null || ta.ChangesTagLevel) {
        _tagLevel--;
      }

      if (_flush) {
        FlushBlock();
      }

      _lastEvent = Event.EndTag;
      _lastEndTag = localName;

      _labelStacks.RemoveLast();
    }

    public override void Characters(char[] characters, int start, int length) {
      _textElementIdx++;
      if (_flush) {
        FlushBlock();
        _flush = false;
      }

      if (InIgnorableElement != 0) {
        return;
      }

      char c;
      bool startWhitespace = false;
      bool endWhitespace = false;
      if (length == 0) {
        return;
      }

      int end = start + length;
      for (int i = start; i < end; i++) {
        if (char.IsWhiteSpace(characters[i])) {
          characters[i] = ' ';
        }
      }
      while (start < end) {
        c = characters[start];
        if (c == ' ') {
          startWhitespace = true;
          start++;
          length--;
        } else {
          break;
        }
      }
      while (length > 0) {
        c = characters[start + length - 1];
        if (c == ' ') {
          endWhitespace = true;
          length--;
        } else {
          break;
        }
      }
      if (length == 0) {
        if (startWhitespace || endWhitespace) {
          if (!SbLastWasWhitespace) {
            _textBuffer.Append(' ');
            TokenBuffer.Append(' ');
          }
          SbLastWasWhitespace = true;
        } else {
          SbLastWasWhitespace = false;
        }
        _lastEvent = Event.Whitespace;
        return;
      }
      if (startWhitespace) {
        if (!SbLastWasWhitespace) {
          _textBuffer.Append(' ');
          TokenBuffer.Append(' ');
        }
      }

      if (_blockTagLevel == -1) {
        _blockTagLevel = _tagLevel;
      }

      _textBuffer.Append(characters, start, length);
      TokenBuffer.Append(characters, start, length);
      if (endWhitespace) {
        _textBuffer.Append(' ');
        TokenBuffer.Append(' ');
      }

      SbLastWasWhitespace = endWhitespace;
      _lastEvent = Event.Characters;

      _currentContainedTextElements.Set(_textElementIdx, true);
    }

    public TextDocument ToTextDocument() {
      // just to be sure
      FlushBlock();

      return new TextDocument(Title, TextBlocks);
    }

    /// <summary>
    ///   Recycles this instance.
    /// </summary>
    public void Recycle() {
      TokenBuffer.Length = 0;
      _textBuffer.Length = 0;

      InBody = 0;
      InAnchor = 0;
      InIgnorableElement = 0;
      SbLastWasWhitespace = false;
      _textElementIdx = 0;

      _textBlocks.Clear();

      _lastStartTag = null;
      _lastEndTag = null;
      _lastEvent = Event.None;

      _offsetBlocks = 0;
      _currentContainedTextElements.SetAll(false);

      _flush = false;
      _inAnchorText = false;
    }

    public void FlushBlock() {
      if (InBody == 0) {
        if ("TITLE".Equals(_lastStartTag, StringComparison.CurrentCultureIgnoreCase) && InBody == 0) {
          Title = TokenBuffer.ToString().Trim();
        }
        _textBuffer.Length = 0;
        TokenBuffer.Length = 0;
        return;
      }

      int length = TokenBuffer.Length;
      switch (length) {
        case 0:
          return;
        case 1:
          if (SbLastWasWhitespace) {
            _textBuffer.Length = 0;
            TokenBuffer.Length = 0;
            return;
          }
          break;
      }
      string[] tokens = UnicodeTokenizer.Tokenize(TokenBuffer);

      int numWords = 0;
      int numLinkedWords = 0;
      int numWrappedLines = 0;
      int currentLineLength = -1; // don't count the first space
      const int maxLineLength = 80;
      int numTokens = 0;
      int numWordsCurrentLine = 0;

      foreach (string token in tokens) {
        if (ANCHOR_TEXT_START.Equals(token)) {
          _inAnchorText = true;
        } else if (ANCHOR_TEXT_END.Equals(token)) {
          _inAnchorText = false;
        } else if (IsWord(token)) {
          numTokens++;
          numWords++;
          numWordsCurrentLine++;
          if (_inAnchorText) {
            numLinkedWords++;
          }
          int tokenLength = token.Length;
          currentLineLength += tokenLength + 1;
          if (currentLineLength > maxLineLength) {
            numWrappedLines++;
            currentLineLength = tokenLength;
            numWordsCurrentLine = 1;
          }
        } else {
          numTokens++;
        }
      }
      if (numTokens == 0) {
        return;
      }
      int numWordsInWrappedLines;
      if (numWrappedLines == 0) {
        numWordsInWrappedLines = numWords;
        numWrappedLines = 1;
      } else {
        numWordsInWrappedLines = numWords - numWordsCurrentLine;
      }

      var tb = new TextBlock(
        _textBuffer.ToString().Trim(),
        _currentContainedTextElements,
        numWords,
        numLinkedWords,
        numWordsInWrappedLines,
        numWrappedLines,
        _offsetBlocks);
      _currentContainedTextElements = new BitArray(short.MaxValue);

      _offsetBlocks++;

      _textBuffer.Length = 0;
      TokenBuffer.Length = 0;

      tb.TagLevel = _blockTagLevel;
      AddTextBlock(tb);
      _blockTagLevel = -1;
    }

    protected void AddTextBlock(TextBlock tb) {
      foreach (var l in FontSizeStack) {
        if (l != null) {
          tb.AddLabel("font-" + l);
          break;
        }
      }
      foreach (var labelStack in _labelStacks) {
        if (labelStack != null) {
          foreach (LabelAction labels in labelStack) {
            if (labels != null) {
              labels.AddTo(tb);
            }
          }
        }
      }

      _textBlocks.Add(tb);
    }

    private static bool IsWord(string token) {
      return PatValidWordCharacter.IsMatch(token);
    }

    public void AddWhitespaceIfNecessary() {
      if (!SbLastWasWhitespace) {
        TokenBuffer.Append(' ');
        _textBuffer.Append(' ');
        SbLastWasWhitespace = true;
      }
    }

    public void AddLabelAction(LabelAction la) {
      LinkedList<LabelAction> labelStack = _labelStacks.Last.Value;
      if (labelStack == null) {
        labelStack = new LinkedList<LabelAction>();
        _labelStacks.RemoveLast();
        _labelStacks.AddLast(labelStack);
      }
      labelStack.AddLast(la);
    }

    private enum Event {
      None,
      StartTag,
      EndTag,
      Characters,
      Whitespace
    }
  }
}
