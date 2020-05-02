using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using GameTiles.Tiles;

namespace CSConsoleRL.Helpers
{
  public sealed class PathfindingHelper
  {
    private static PathfindingHelper _instance;
    private readonly TileTypeDictionary _tileDict;
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

    private PathfindingHelper()
    {
      _tileDict = new TileTypeDictionary();
    }

    private int CalculateH(int x, int y, int endX, int endY)
    {
      int h = Math.Abs(x - endX) + Math.Abs(y - endY);

      return h;
    }

    private PathfindingNode GetExistingPathfindingNode(List<PathfindingNode> list, int x, int y)
    {
      return list.Where(node => node.X == x && node.Y == y).FirstOrDefault();
    }

    public List<Vector2i> Path(Tile[,] gameTiles, Vector2i start, Vector2i end)
    {
      var openPath = new List<PathfindingNode>() {
        new PathfindingNode(start.X, start.Y, 0, CalculateH(start.X, start.Y, end.X, end.Y), null)
      };
      var closedPath = new List<PathfindingNode>();

      while (openPath.Count > 0)
      {
        var current = openPath[0];

        // Find lowest value node
        foreach (var node in openPath)
        {
          if (node.F < current.F)
          {
            current = node;
          }
        }

        openPath.Remove(current);
        closedPath.Add(current);

        // Reached goal co-ordinates, build list back to start
        if (current.X == end.X && current.Y == end.Y)
        {
          var path = new List<Vector2i>() { new Vector2i(current.X, current.Y) };

          var parent = current.Parent;
          while (parent != null)
          {
            path.Add(new Vector2i(parent.X, parent.Y));
            parent = parent.Parent;
          }
          // Remove last tile as is starting one
          path.RemoveAt((path.Count - 1));
          path.Reverse();

          return path;
        }

        for (int dir = 0; dir <= 3; dir++)
        {
          int x = 0, y = 0;
          switch (dir)
          {
            case 0: // Above
              x = current.X;
              y = current.Y - 1;
              if (y < 0) continue;
              break;
            case 1: // Below
              x = current.X;
              y = current.Y + 1;
              if (y >= gameTiles.GetLength(1)) continue;
              break;
            case 2: // Left
              x = current.X - 1;
              y = current.Y;
              if (x < 0) continue;
              break;
            case 3: // Right
              x = current.X + 1;
              y = current.Y;
              if (x >= gameTiles.GetLength(0)) continue;
              break;
          }

          if (GetExistingPathfindingNode(closedPath, x, y) != null) continue;

          if (!_tileDict[gameTiles[x, y].TileType].BlocksMovement)
          {
            var g = current.G + 1;
            var h = CalculateH(x, y, end.X, end.Y);

            // Check if node already exists in openList
            var existing = GetExistingPathfindingNode(openPath, x, y);

            // If it does but this has lower cost, update node
            // Otherwise create new PathfindingNode
            if (existing != null)
            {
              if (g + h < existing.F)
              {
                existing.Parent = current;
                existing.G = g;
              }
            }
            else
            {
              var newNode = new PathfindingNode(x, y, g, h, current);
              openPath.Add(newNode);
            }
          }
        }
      }

      return null;
    }

    private class PathfindingNode
    {
      public int X { get; private set; }
      public int Y { get; private set; }
      public int F { get { return G + H; } }  // Determined cost of path
      public int G { get; set; }  // Cost of path from start to this node
      public int H { get; set; }  // Heuristic cost of direct path to goal tile
      public PathfindingNode Parent { get; set; }

      public PathfindingNode(int x, int y, int g, int h, PathfindingNode parent)
      {
        X = x;
        Y = y;
        G = g;
        H = h;
        Parent = parent;
      }
    }
  }
}
