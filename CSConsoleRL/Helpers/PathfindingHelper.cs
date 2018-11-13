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
            public int Cost;

            public PathfindingNode(int x, int y)
            {
                Coord = new Vector2i(x, y);
                Cost = -1;
                Children = new List<PathfindingNode>();
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
                    return LowestCostNode?.Cost;
                }
            }
            public int Count
            {
                get
                {
                    return _list == null ? 0 : _list.Count;
                }
            }

            public PathfindingList()
            {
                _list = new List<PathfindingNode>();
            }

            private void CalculateLowestCostNode()
            {
                if (_list.Count == 0) return;

                var lowestNode = _list[0];

                for (int i = 1; i < _list.Count; i++)
                {
                    if (_list[i].Cost < lowestNode.Cost)
                    {
                        lowestNode = _list[i];
                    }
                }
            }

            public PathfindingNode Add(PathfindingNode node)
            {
                node.Cost += (Math.Abs(node.Coord.X - TargetCoords.X)) ^ 2 + (Math.Abs(node.Coord.Y - TargetCoords.Y)) ^ 2;
                _list.Add(node);

                if (node.Cost < LowestCostNode.Cost)
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

                if (LowestCostNode == node)
                {
                    CalculateLowestCostNode();
                }

                return node;
            }

            public List<Vector2i> GetOpenPath()
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

        public List<Vector2i> Path(Tile[,] gameTiles, Vector2i start, Vector2i end)
        {
            var openPath = new PathfindingList();
            var closedPath = new List<PathfindingNode>();

            //Initialize openPath with start tile
            var currentNode = openPath.Add(new PathfindingNode(start.X, start.Y));

            //Main loop, finishes when end is found
            while (openPath.Count > 0)
            {

            }

            return openPath;
        }
    }
}
