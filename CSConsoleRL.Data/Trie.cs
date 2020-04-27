using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSConsoleRL.Data
{
  public class Trie
  {
    private TrieNode _rootNode;

    public Trie()
    {
      _rootNode = new TrieNode(true, null);
    }

    public void Insert(string value)
    {
      _rootNode.Insert(value);
    }

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

      /// <summary>
      /// Finds all strings in Trie that start with specified value
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
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
          // We've reached the end of the Trie branch, just return input string
          return new List<string>() { originalValue };
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