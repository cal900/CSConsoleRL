using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using GameTiles.Tiles;

namespace CSConsoleRL.Helpers
{
    public class PathfindingHelper
    {
        private static PathfindingHelper _instance;
        private TileTypeDictionary _tileDict;

        private class PathfindingNode
        {
            public Vector2i Coord;
            public List<PathfindingNode> Children;
            public int F { get { return G + H; } }
            public int G { get; set; }
            public int H { get; set; }
            public PathfindingNode Parent { get; private set; }

            public PathfindingNode(int x, int y, PathfindingNode parent)
            {
                Coord = new Vector2i(x, y);
                G = 0;
                Children = new List<PathfindingNode>();

                if (parent != null)
                {
                    G = parent.G + 1;
                    parent.Children.Add(this);
                    Parent = parent;
                }
            }

            public void UpdateParent(PathfindingNode newParent)
            {
                Parent = newParent;
                G = Parent.G + 1;

                //Need to update all children as G cost decrease cascades down
                foreach (var child in Children)
                {
                    child.UpdateParent(this);
                }
            }
        }

        private class PathfindingList
        {
            private List<PathfindingNode> _list;
            public PathfindingNode LowestCostNode { get; private set; }
            public Vector2i TargetCoords;
            public int? LowestCostValue
            {
                get
                {
                    return LowestCostNode?.F;
                }
            }
            public int Count
            {
                get
                {
                    return _list == null ? 0 : _list.Count;
                }
            }

            public PathfindingList(Vector2i target)
            {
                TargetCoords = target;
                _list = new List<PathfindingNode>();
            }

            public PathfindingNode GetNodeByCoords(Vector2i coords)
            {
                foreach (var listItem in _list)
                {
                    if(listItem.Coord.X == coords.X && listItem.Coord.Y == coords.Y)
                    {
                        return listItem;
                    }
                }

                return null;
            }

            private void CalculateLowestCostNode()
            {
                if (_list.Count == 0)
                {
                    LowestCostNode = null;
                    return;
                }

                var lowestNode = _list[0];

                for (int i = 1; i < _list.Count; i++)
                {
                    if (_list[i].F < lowestNode.F)
                    {
                        lowestNode = _list[i];
                    }
                }

                LowestCostNode = lowestNode;
            }

            public PathfindingNode Add(PathfindingNode node)
            {
                int horDist = Math.Abs(node.Coord.X - TargetCoords.X);
                int verDist = Math.Abs(node.Coord.Y - TargetCoords.Y);
                node.H = horDist > verDist ? horDist : verDist;

                if(ContainsCoords(new Vector2i(node.Coord.X, node.Coord.Y)))
                {
                    throw new Exception(string.Format("List already contains a node at co-ordinate {0}, {1}", node.Coord.X, node.Coord.Y));
                }

                _list.Add(node);

                if(LowestCostNode == null)
                {
                    LowestCostNode = node;
                }
                else if (node.F <= LowestCostNode.F)
                {
                    LowestCostNode = node;
                }

                return node;
            }

            public PathfindingNode Remove(PathfindingNode node)
            {
                if (_list.Contains(node))
                {
                    _list.Remove(node);
                }
                else
                {
                    throw new Exception(string.Format("node with co-ordinates {0}, {1} not found in list of size {2}", node.Coord.X, node.Coord.Y, _list.Count));
                }

                if (LowestCostNode == node)
                {
                    CalculateLowestCostNode();
                }

                return node;
            }

            public bool Contains(PathfindingNode node)
            {
                return _list.Contains(node);
            }

            public bool ContainsCoords(Vector2i coords)
            {
                foreach (var listItem in _list)
                {
                    if(listItem.Coord.X == coords.X && listItem.Coord.Y == coords.Y)
                    {
                        return true;
                    }
                }

                return false;
            }

            public List<Vector2i> GetVectorList()
            {
                var openPath = new List<Vector2i>();
                foreach (var node in _list)
                {
                    openPath.Add(node.Coord);
                }

                return openPath;
            }
        }

        private PathfindingHelper()
        {
            _tileDict = new TileTypeDictionary();
        }

        public static PathfindingHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PathfindingHelper();
                }
                return _instance;
            }
        }

        private bool ListContainsCoords(List<Vector2i> list, int x, int y)
        {
            foreach (var listItem in list)
            {
                if (listItem.X == x && listItem.Y == y)
                    return true;
            }

            return false;
        }
        private List<Vector2i> BuildPathFromNodeChain(PathfindingNode node)
        {
            var list = new List<Vector2i>();

            while(node != null)
            {
                list.Add(node.Coord);
                node = node.Parent;
            }

            list.Reverse();

            //Remove last co-ordinate which will just correspond to current location
            list.RemoveAt(list.Count - 1);

            return list;
        }

        public List<Vector2i> Path(Tile[,] gameTiles, Vector2i start, Vector2i end)
        {
            var openPath = new PathfindingList(end);
            var closedPath = new List<Vector2i>();

            //Initialize openPath with start tile
            openPath.Add(new PathfindingNode(start.X, start.Y, null));

            //Main loop, finishes when end is found
            while (openPath.Count > 0)
            {
                var currentNode = openPath.LowestCostNode;
                openPath.Remove(currentNode);
                closedPath.Add(currentNode.Coord);

                int startY = currentNode.Coord.Y - 1;
                if (startY < 0) startY = 0;
                int endY = currentNode.Coord.Y + 1;
                if (endY >= 30) endY = 29;
                int startX = currentNode.Coord.X - 1;
                if (startX < 0) startX = 0;
                int endX = currentNode.Coord.X + 1;
                if (endX >= 30) endX = 29;

                //Look at all adjacent tiles
                for (int y = startY; y <= endY; y++)
                {
                    for (int x = startX; x <= endX; x++)
                    {
                        //This check excludes the current node co-ordinates
                        if (!(x == currentNode.Coord.X && y == currentNode.Coord.Y))
                        {
                            //Reached goal co-ordinates
                            if(currentNode.Coord.X == end.X && currentNode.Coord.Y == end.Y)
                            {
                                return BuildPathFromNodeChain(currentNode);
                            }
                            else if (!_tileDict[gameTiles[x, y].TileType].BlocksMovement
                                && !ListContainsCoords(closedPath, x, y))
                            {
                                var existingNode = openPath.GetNodeByCoords(new Vector2i(x, y));
                                if (existingNode == null)
                                {
                                    openPath.Add(new PathfindingNode(x, y, currentNode));
                                }
                                else
                                {
                                    //If openPath already contains node, and if this path has smaller cost update the node
                                    int h = Math.Abs(x - end.X) < Math.Abs(y - end.Y) ? Math.Abs(x - end.X) : Math.Abs(y - end.Y);
                                    int f = (currentNode.G + 1) + h;
                                    if(f < existingNode.F)
                                    {
                                        existingNode.UpdateParent(currentNode);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //If path not found just return null
            return null;
        }
    }
}
