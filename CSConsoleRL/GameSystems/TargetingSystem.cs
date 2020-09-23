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
using CSConsoleRL.Helpers;

namespace CSConsoleRL.GameSystems
{
  public class TargetingSystem : GameSystem
  {
    private Vector2i _startingCoords;
    private Vector2i _targetedCoords;
    private List<Vector2i> _targetingPath;
    private GameStateHelper _gameStateHelper;

    public TargetingSystem(GameSystemManager manager, Tile[,] _gameTiles, GameStateHelper gameStateHelper)
    {
      SystemManager = manager;
      _systemEntities = new List<Entity>();
      _targetedCoords = new Vector2i(0, 0);
      _targetingPath = new List<Vector2i>();
      _gameStateHelper = gameStateHelper;
    }

    public override void InitializeSystem()
    {
      //_targetingPath = PlotCourse((new Vector2i(0, 0)), new Vector2i(15, 10));
    }

    public override void HandleMessage(IGameEvent gameEvent)
    {
      switch (gameEvent.EventName)
      {
        case "NotifyChangeGameState":
          var newState = (EnumGameState)gameEvent.EventParams[0];
          if (newState == EnumGameState.Targeting)
          {
            StartTargetingMode();
          }
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

    private void StartTargetingMode()
    {
      _startingCoords.X = _gameStateHelper.CameraCoords.X;
      _startingCoords.Y = _gameStateHelper.CameraCoords.Y;
      _targetedCoords.X = _gameStateHelper.CameraCoords.X;
      _targetedCoords.Y = _gameStateHelper.CameraCoords.Y;
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
      if (Math.Abs(xDiff) >= Math.Abs(yDiff))
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
          var x = (int)Math.Round((double)y / slope);

          path.Add(new Vector2i(start.X + x, start.Y + y));
        }
      }

      return path;
    }
  }
}