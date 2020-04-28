using System.Collections.Generic;

namespace CSConsoleRL.Data
{
  public class Trie
  {
    private readonly TrieNode _rootNode;

    public Trie()
    {
      _rootNode = new TrieNode(true, null);
    }

    /// <summary>
    /// Adds value to the Trie
    /// </summary>
    /// <param name="value"></param>
    public void Insert(string value)
    {
      _rootNode.Insert(value);
    }

    /// <summary>
    /// Get all strings in Trie that start with the specified value.
    /// If the value doesn't exist  at all in Trie will return null.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public List<string> GetBranches(string value)
    {
      return _rootNode.GetBranches(value, value);
    }

    private class TrieNode
    {
      private readonly bool _rootNode;
      private readonly char? _char;
      private readonly Dictionary<char, TrieNode> _children;

      public TrieNode(bool rootNode, char? nodeChar)
      {
        _rootNode = rootNode;
        _char = nodeChar;
        _children = new Dictionary<char, TrieNode>();
      }

      public void Insert(string value)
      {
        // We've fully inserted value into the Trie; add null node to indicate an end
        if (value == "")
        {
          _children.Add(' ', new TrieNode(false, null));
          return;
        }

        var childChar = value[0];

        if (!_children.ContainsKey(childChar))
        {
          _children.Add(childChar, new TrieNode(false, childChar));
        }

        _children[childChar].Insert(value.Substring(1));
      }

      public List<string> GetBranches(string originalValue, string value)
      {
        // We've reached the end of the input string, return all branches from this node
        if (value == "")
        {
          var branches = new List<string>();
          GetAllBranches(branches, originalValue.Substring(0, originalValue.Length - 1));
          return branches;
        }

        var childChar = value[0];

        if (_children.ContainsKey(childChar))
        {
          return _children[childChar].GetBranches(originalValue, value.Substring(1));
        }
        else
        {
          // We've reached the end of the Trie branch with still
          // chars to go in value, return null
          return null;
        }
      }

      private void GetAllBranches(List<string> branches, string valueSoFar)
      {
        if (this._char == null)
        {
          branches.Add(valueSoFar);
        }
        else
        {
          valueSoFar += this._char;

          foreach (var key in _children.Keys)
          {
            _children[key].GetAllBranches(branches, valueSoFar);
          }
        }
      }
    }
  }
}