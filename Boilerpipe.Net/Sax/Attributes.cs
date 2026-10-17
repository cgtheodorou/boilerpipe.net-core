namespace Boilerpipe.Net.Sax {
  using System;
  using System.Collections.Generic;

  internal sealed class Attributes : IAttributes {
    private readonly List<KeyValuePair<string, string>> _attributes = new List<KeyValuePair<string, string>>();

    public int Length => _attributes.Count;

    public void Add(string name, string value) {
      _attributes.Add(new KeyValuePair<string, string>(name, value));
    }

    public string GetLocalName(int index) => _attributes[index].Key;

    public string GetQName(int index) => _attributes[index].Key;

    public string GetValue(int index) => _attributes[index].Value;

    public int GetIndex(string qName) => _attributes.FindIndex(a => string.Equals(a.Key, qName, StringComparison.Ordinal));

    public string GetValue(string qName) {
      int index = GetIndex(qName);
      return index < 0 ? null : _attributes[index].Value;
    }
  }
}
