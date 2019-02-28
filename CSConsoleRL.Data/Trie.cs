using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSConsoleRL.Data
{
    /// <summary>
    /// A space char is used to indicate that branch level is also an endpoint
    /// </summary>
    public class Trie
    {
        private Dictionary<char, TrieBranch> _branches;

        public Trie()
        {
            _branches = new Dictionary<char, TrieBranch>();
        }

        public void Insert(string remainingBranches)
        {
            if(remainingBranches.Length > 0)
            {
                if(_branches.ContainsKey(remainingBranches[0]))
                {
                    _branches[remainingBranches[0]].Insert(remainingBranches);
                }
                else
                {
                    _branches.Add(remainingBranches[0], new TrieBranch(remainingBranches));
                }
            }
        }

        public List<string> GetPathBranches(string path)
        {
            var pathBranches = new List<string>();

            return null;
        }

        private class TrieBranch
        {
            private Dictionary<char, TrieBranch> _branches;
            public readonly char Value;

            public TrieBranch(string remainingBranches)
            {
                Value = remainingBranches[0];

                Insert(remainingBranches.Substring(1));
            }

            public void Insert(string remainingBranches)
            {
                if (remainingBranches.Length > 0)
                {
                    if (_branches.ContainsKey(remainingBranches[0]))
                    {
                        _branches[remainingBranches[0]].Insert(remainingBranches.Substring(1));
                    }
                    else
                    {
                        _branches.Add(remainingBranches[0], new TrieBranch(remainingBranches.Substring(1)));
                    }
                }
            }

            public void GetAllChildBranches(string localPath, List<string> paths)
            {
                if(_branches.Count == 0)
                {
                    paths.Add(localPath + Value);
                }
                else
                {
                    foreach (var branch in _branches)
                    {
                        GetAllChildBranches(localPath + Value, paths);
                    }
                }
            }

            public void GetPathBranches(string path, string fullPath, List<string> branches)
            {
                if(path.Length > 0)
                {
                    if(_branches.ContainsKey(path[0]))
                    {
                        //Go into child branch
                        GetPathBranches(path.Substring(1), fullPath, branches);
                    }
                    else
                    {
                        //Reached end of road
                        return;
                    }
                }
                //If reached end of the road return all child branches
                else
                {
                    foreach (KeyValuePair<char, TrieBranch> branch in _branches)
                    {
                        //branches.Add(branch.)
                    }
                }
            }
        }
    }
}

//Navigate until hit end of string, then return all remaining branches