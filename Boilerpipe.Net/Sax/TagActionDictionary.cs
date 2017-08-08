namespace Boilerpipe.Net.Sax {
  using System.Collections.Generic;

  /// <summary>
  ///   Base class for definition a set of <see cref="ITagAction" />s that are to be used for the HTML parsing process.
  /// </summary>
  /// <seealso cref="DefaultTagActionDictionary" />
  public abstract class TagActionDictionary : Dictionary<string, ITagAction> {
    /// <summary>
    ///   Sets a particular <see cref="ITagAction" /> for a given tag. Any existing TagAction for that tag
    ///   will be removed and overwritten.
    /// </summary>
    /// <param name="tag">The tag (will be stored internally 1. as it is, 2. lower-case, 3. upper-case)</param>
    /// <param name="action">The <see cref="ITagAction" /></param>
    protected void SetTagAction(string tag, ITagAction action) {
      this[tag.ToUpperInvariant()] = action;
      this[tag.ToLowerInvariant()] = action;
      this[tag] = action;
    }

    /// <summary>
    ///   Adds a particular <see cref="ITagAction" /> for a given tag. If a TagAction already exists for that tag,
    ///   a chained action, consisting of the previous and the new <see cref="ITagAction" /> is created.
    /// </summary>
    /// <param name="tag">The tag (will be stored internally 1. as it is, 2. lower-case, 3. upper-case)</param>
    /// <param name="action">The <see cref="ITagAction" /></param>
    protected void AddTagAction(string tag, ITagAction action) {
      ITagAction previousAction;
      if (TryGetValue(tag, out previousAction)) {
        SetTagAction(tag, new CommonTagActions.Chained(previousAction, action));
      } else {
        SetTagAction(tag, action);
      }
    }
  }
}
