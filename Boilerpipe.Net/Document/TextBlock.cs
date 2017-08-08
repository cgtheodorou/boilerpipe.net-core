namespace Boilerpipe.Net.Document {
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using System.Text;

  using Boilerpipe.Net.Labels;
    using Boilerpipe.Net.Util;

    /// <summary>
    ///   Describes a block of text.
    ///   A block can be an "atomic" text element (i.e., a sequence of text that is not
    ///   interrupted by any HTML markup) or a compound of such atomic elements.
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
  public class TextBlock
    {
    private static readonly BitArray EmptyBitset = new BitArray(0);
    public static readonly TextBlock EmptyStart = new TextBlock("", EmptyBitset, 0, 0, 0, 0, -1);
    public static readonly TextBlock EmptyEnd = new TextBlock("", EmptyBitset, 0, 0, 0, 0, int.MaxValue);

    private readonly StringBuilder _text;
    private BitArray _containedTextElements;
    private List<string> _labels;
    private float _linkDensity;
    private int _numFullTextWords;
    private int _numWords;
    private int _numWordsInAnchorText;
    private int _numWordsInWrappedLines;
    private int _numWrappedLines;
    private int _offsetBlocksEnd;
    private int _offsetBlocksStart;
    private int _tagLevel;
    private float _textDensity;

    public TextBlock(string text)
      : this(text, null, 0, 0, 0, 0, 0) {
    }

    public TextBlock(
      string text,
      BitArray containedTextElements,
      int numWords,
      int numWordsInAnchorText,
      int numWordsInWrappedLines,
      int numWrappedLines,
      int offsetBlocks) {
      IsContent = false;
      _text = new StringBuilder(text);
      _containedTextElements = containedTextElements;
      _numWords = numWords;
      _numWordsInAnchorText = numWordsInAnchorText;
      _numWordsInWrappedLines = numWordsInWrappedLines;
      _numWrappedLines = numWrappedLines;
      _offsetBlocksStart = offsetBlocks;
      _offsetBlocksEnd = offsetBlocks;
      InitDensities();
    }

    public bool IsContent { get; set; }

    public string Text {
      get {
        return _text.ToString();
      }
    }

    public int NumWords {
      get {
        return _numWords;
      }
    }

    public int NumWordsInAnchorText {
      get {
        return _numWordsInAnchorText;
      }
    }

    public float TextDensity {
      get {
        return _textDensity;
      }
    }

    public float LinkDensity {
      get {
        return _linkDensity;
      }
    }

    public int OffsetBlocksStart {
      get {
        return _offsetBlocksStart;
      }
    }

    public int OffsetBlocksEnd {
      get {
        return _offsetBlocksEnd;
      }
    }

    /// <summary>
    ///   Returns the labels associated to this <see cref="TextBlock" />, or <code>null</code> if no such labels exist.
    /// </summary>
    /// <remarks>
    ///   NOTE: The returned instance is the one used directly in <see cref="TextBlock" />. You have full access
    ///   to the data structure. However it is recommended to use the label-specific methods in <see cref="TextBlock" />
    ///   whenever possible.
    /// </remarks>
    public IEnumerable<string> Labels {
      get {
        return _labels;
      }
    }

    /// <summary>
    ///   Returns the containedTextElements BitArray, or <code>null</code>.
    /// </summary>
    public BitArray ContainedTextElements {
      get {
        return _containedTextElements;
      }
    }

    public int TagLevel {
      get {
        return _tagLevel;
      }
      set {
        _tagLevel = value;
      }
    }

    public object Clone() {
      var clone = (TextBlock)MemberwiseClone();

      if (_labels != null && _labels.Count != 0) {
        clone._labels = new List<String>(_labels);
      }
      if (_containedTextElements != null) {
        clone._containedTextElements = (BitArray)_containedTextElements;
      }

      return clone;
    }


    /// <summary>
    /// Merges specified <see cref="TextBlock"/> with this <see cref="TextBlock"/>.
    /// </summary>
    /// <param name="other">Then <see cref="TextBlock"/> to merge with.</param>
    public void MergeNext(TextBlock other) {
      StringBuilder sb = _text;
      sb.Append('\n');
      sb.Append(other._text);

      _numWords += other._numWords;
      _numWordsInAnchorText += other._numWordsInAnchorText;

      _numWordsInWrappedLines += other._numWordsInWrappedLines;
      _numWrappedLines += other._numWrappedLines;

      _offsetBlocksStart = Math.Min(_offsetBlocksStart, other._offsetBlocksStart);
      _offsetBlocksEnd = Math.Max(_offsetBlocksEnd, other._offsetBlocksEnd);

      InitDensities();

      IsContent |= other.IsContent;

      if (_containedTextElements != null) { 
        _containedTextElements.Or(other._containedTextElements);
      }

      _numFullTextWords += other._numFullTextWords;

      if (other._labels != null) {
        if (_labels == null) {
          _labels = new List<String>(other._labels);
        } else {
          _labels.AddRange(other._labels);
        }
      }

      _tagLevel = Math.Min(_tagLevel, other._tagLevel);
    }

    private void InitDensities() {
      if (_numWordsInWrappedLines == 0) {
        _numWordsInWrappedLines = _numWords;
        _numWrappedLines = 1;
      }
      _textDensity = _numWordsInWrappedLines / (float)_numWrappedLines;
      _linkDensity = _numWords == 0 ? 0 : _numWordsInAnchorText / (float)_numWords;
    }

    public override string ToString() {
      return "[" + _offsetBlocksStart + "-" + _offsetBlocksEnd + ";tl=" + _tagLevel + "; nw=" + _numWords + ";nwl="
             + _numWrappedLines + ";ld=" + _linkDensity + "]\t" + (IsContent ? "CONTENT" : "boilerplate") + ","
             + string.Join(",", Labels ?? Enumerable.Empty<string>()) + "\n" + Text;
    }

    /// <summary>
    ///   Adds an arbitrary String label to this <see cref="TextBlock" />.
    /// </summary>
    /// <param name="label">The label to be added</param>
    /// <remarks>
    ///   <seealso cref="DefaultLabels" />
    /// </remarks>
    public void AddLabel(string label) {
      if (_labels == null) {
        _labels = new List<String>(2);
      }
      _labels.Add(label);
    }

    /// <summary>
    ///   Checks whether this <see cref="TextBlock" /> has the given label.
    /// </summary>
    /// <param name="label">The label to check</param>
    /// <returns><code>true</code> if this block is marked by the given label.</returns>
    public bool HasLabel(string label) {
      return _labels != null && _labels.Contains(label);
    }

    /// <summary>
    ///   Removes a label from this <see cref="TextBlock" />.
    /// </summary>
    /// <param name="label">The label to be removed.</param>
    /// <returns>Returns <code>true</code> if the label have been removed.</returns>
    public bool RemoveLabel(string label) {
      return _labels != null && _labels.Remove(label);
    }

    /// <summary>
    ///   Adds a set of labels to this <see cref="TextBlock" />. <code>null</code>-references are silently ignored.
    /// </summary>
    /// <param name="labels">The Labels to be added</param>
    public void AddLabels(IEnumerable<string> labels) {
      if (labels == null) {
        return;
      }
      if (_labels == null) {
        _labels = new List<String>(labels);
      } else {
        _labels.AddRange(labels);
      }
    }

    /// <summary>
    ///   Adds a set of labels to this <see cref="TextBlock" />. <code>null</code>-references are silently ignored.
    /// </summary>
    /// <param name="labels">The Labels to be added</param>
    public void AddLabels(params string[] labels) {
      if (labels == null) {
        return;
      }
      if (_labels == null) {
        _labels = new List<String>();
      }
      _labels.AddRange(labels);
    }
  }
}
