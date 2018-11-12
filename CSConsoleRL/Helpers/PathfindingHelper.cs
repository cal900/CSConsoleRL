﻿using System;
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
            public PathfindingNode LowestCostNode;
            public int? LowestCostValue
            {
                get
                {
                    return LowestCostNode?.Cost;
                }
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

            public void Add(PathfindingNode node)
            {
                _list.Add(node);

                if(node.Cost < LowestCostNode.Cost)
                {
                    LowestCostNode = node;
                }
            }

            public PathfindingNode Remove(PathfindingNode node)
            {
                if(_list.Contains(node))
                {
                    _list.Remove(node);
                }

                if(LowestCostNode == node)
                {
                    CalculateLowestCostNode();
                }

                return node;
            }

            public PathfindingNode RemoveLowestCostNode()
            {
                if(LowestCostNode == null)
                {
                    return null;
                }
                else
                {
                    return Remove(LowestCostNode);
                }
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
                if(_instance == null)
                {
                    _instance = new PathfindingHelper();
                }
                return _instance;
            }
        }

        public List<Vector2i> Path(Tile[,] gameTiles, Vector2i start, Vector2i end)
        {
            var openPath = new List<PathfindingNode>();
            var closedPath = new List<PathfindingNode>();

            //Initialize openPath with start tile
            openPath.Add(new PathfindingNode(start.X, start.Y));

            //Main loop, finishes when end is found
            while(openPath.Count > 0)
            {

            }

            return openPath;
        }
    }
}
