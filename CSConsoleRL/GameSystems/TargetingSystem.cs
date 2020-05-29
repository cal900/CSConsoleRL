using CSConsoleRL.Components;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;
using CSConsoleRL.Enums;
using CSConsoleRL.Game.Managers;
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
    private Vector2i _targetedCoords;
    private List<Vector2i> _targetingPath;

    public TargetingSystem(GameSystemManager manager)
    {
      SystemManager = manager;
      _systemEntities = new List<Entity>();
      _targetedCoords = new Vector2i(0, 0);
      _targetingPath = new List<Vector2i>();
    }

    public override void InitializeSystem()
    {

    }

    public override void HandleMessage(IGameEvent gameEvent)
    {
      switch (gameEvent.EventName)
      {
        case "MoveTargetingCursor":
          var dir = (EnumDirections)gameEvent.EventParams[0];
          MoveTargetingCursor(dir);
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
          _targetedCoords.Y++;
          break;
        case EnumDirections.South:
          _targetedCoords.Y--;
          break;
        case EnumDirections.East:
          _targetedCoords.X++;
          break;
        case EnumDirections.West:
          _targetedCoords.X--;
          break;
      }

      _targetingPath = PlotCourse(null, _targetedCoords);
    }

    private List<Vector2i> PlotCourse(Vector2i start, Vector2i end)
    {
      var path = new List<Vector2i>();
      var xDiff = end.X - start.X;
      var yDiff = end.Y - start.Y;
      var slope = (double)yDiff / (double)xDiff;

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