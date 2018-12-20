using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSConsoleRL.Data
{
    public class Trie
    {
        private Dictionary<char, TrieBranch> branches;

        public Trie()
        {
            branches = new Dictionary<char, TrieBranch>();
        }

        public void Insert(string remainingBranches)
        {
            if(remainingBranches.Length > 0)
            {
                if(branches.ContainsKey(remainingBranches[0]))
                {
                    branches[remainingBranches[0]].Insert(remainingBranches);
                }
                else
                {
                    branches.Add(remainingBranches[0], new TrieBranch(remainingBranches));
                }
            }
        }

        public List<string> GetPathBranches(string path)
        {
            var pathBranches = new List<string>();


        }

        private class TrieBranch
        {
            private Dictionary<char, TrieBranch> branches;
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
                    if (branches.ContainsKey(remainingBranches[0]))
                    {
                        branches[remainingBranches[0]].Insert(remainingBranches.Substring(1));
                    }
                    else
                    {
                        branches.Add(remainingBranches[0], new TrieBranch(remainingBranches.Substring(1)));
                    }
                }
            }

            public List<string> GetPathBranches(string path, List<string> branches)
            {
                
            }
        }
    }
}
