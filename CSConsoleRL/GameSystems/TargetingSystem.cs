using CSConsoleRL.Components;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;
using CSConsoleRL.Enums;
using CSConsoleRL.Game.Managers;
using GameTiles.Tiles;
using SFML.Window;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSConsoleRL.GameSystems
{
  public class TargetingSystem : GameSystem
  {
    private Vector2i _startingCoords;
    private Vector2i _targetedCoords;
    private List<Vector2i> _targetingPath;

    public TargetingSystem(GameSystemManager manager, Tile[,] _gameTiles)
    {
      SystemManager = manager;
      _systemEntities = new List<Entity>();
      _targetedCoords = new Vector2i(0, 0);
      _targetingPath = new List<Vector2i>();
    }

    public override void InitializeSystem()
    {
      _targetingPath = PlotCourse((new Vector2i(0, 0)), new Vector2i(15, 10));
    }

    public override void HandleMessage(IGameEvent gameEvent)
    {
      switch (gameEvent.EventName)
      {
        case "StartTargetingMode":
          var startPos = (Vector2i)gameEvent.EventParams[0];
          _startingCoords = startPos;
          _targetedCoords = startPos;
          break;
        case "MoveTargetingCursor":
          var dir = (EnumDirections)gameEvent.EventParams[0];
          MoveTargetingCursor(dir);
          break;
        case "RequestTargetingPath":
          BroadcastMessage(new SendTargetingPathEvent(_targetingPath));
          break;
      }
    }

    public override void AddEntity(Entity entity)
    {

    }

    private void MoveTargetingCursor(EnumDirections dir)
    {
      switch (dir)
      {
        case EnumDirections.North:
          _targetedCoords.Y--;
          break;
        case EnumDirections.South:
          _targetedCoords.Y++;
          break;
        case EnumDirections.West:
          _targetedCoords.X--;
          break;
        case EnumDirections.East:
          _targetedCoords.X++;
          break;
      }

      _targetingPath = PlotCourse(_startingCoords, _targetedCoords);
    }

    private List<Vector2i> PlotCourse(Vector2i start, Vector2i end)
    {
      var path = new List<Vector2i>();
      var xDiff = end.X - start.X;
      var yDiff = end.Y - start.Y;
      var slope = 0D;

      if (xDiff == 0 && yDiff == 0)
      {
        path.Add(new Vector2i(start.X, start.Y));
        return path;
      }
      else if (xDiff == 0)
      {
        slope = 10 * 1000;
      }
      else if (yDiff == 0)
      {
        slope = 0;
      }
      else
      {
        slope = (double)yDiff / (double)xDiff;
      }

      // We use the smaller dimension as a base for plotting course
      if (xDiff >= yDiff)
      {
        for (int x = 0; x < xDiff; x++)
        {
          var y = (int)Math.Round((double)x * slope);

          path.Add(new Vector2i(start.X + x, start.Y + y));
        }
      }
      else
      {
        for (int y = 0; y < yDiff; y++)
        {
          var x = (int)Math.Round((double)y * slope);

          path.Add(new Vector2i(start.X + x, start.Y + y));
        }
      }

      return path;
    }
  }
}