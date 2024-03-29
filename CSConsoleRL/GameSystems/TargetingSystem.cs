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
using CSConsoleRL.Data;

namespace CSConsoleRL.GameSystems
{
  public class TargetingSystem : GameSystem
  {
    private Vector2i _startingCoords;
    private Vector2i _targetedCoords;
    private int _targetingRange;
    private readonly GameStateHelper _gameStateHelper;
    private readonly Tile[,] _gameTiles;
    private readonly TileTypeDictionary _tileDictionary;

    public TargetingSystem(GameSystemManager manager, Tile[,] gameTiles, GameStateHelper gameStateHelper)
    {
      SystemManager = manager;
      _systemEntities = new List<Entity>();
      _targetedCoords = new Vector2i(0, 0);
      _gameStateHelper = gameStateHelper;
      _gameTiles = gameTiles;
      _tileDictionary = new TileTypeDictionary();
      StopTargetingMode();
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
          else
          {
            StopTargetingMode();
          }
          break;
        case "MoveTargetingCursor":
          var dir = (EnumDirections)gameEvent.EventParams[0];
          MoveTargetingCursor(dir);
          break;
        case "PlayerRequestAttack":
          PlayerRequestAttack();
          break;
        case "EntityRequestAttack":
          EntityRequestAttack((EntityRequestAttackEvent)gameEvent);
          break;
      }
    }

    public override void AddEntity(Entity entity)
    {

    }

    private void StartTargetingMode()
    {
      // When toggle to targeting mode targeting should default to character position
      var mainChar = _gameStateHelper.GetVar<ActorEntity>("MainEntity");
      var charPos = mainChar.GetComponent<PositionComponent>();
      var activeItem = mainChar.GetComponent<InventoryComponent>().GetActiveItem();

      _startingCoords.X = charPos.ComponentXPositionOnMap;
      _startingCoords.Y = charPos.ComponentYPositionOnMap;
      _targetedCoords.X = charPos.ComponentXPositionOnMap;
      _targetedCoords.Y = charPos.ComponentYPositionOnMap;
      _gameStateHelper.SetVar("TargetingData", new TargetingData(new List<Vector2i>(), new Vector2i(_targetedCoords.X, _targetedCoords.Y)));

      if (activeItem.GetType() == typeof(Weapon))
      {
        _targetingRange = ((Weapon)activeItem).Range;
      }
      else
      {
        _targetingRange = 0;
      }
    }

    private void StopTargetingMode()
    {
      _gameStateHelper.SetVar("TargetingData", new TargetingData(new List<Vector2i>(), new Vector2i()));
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

      _gameStateHelper.SetVar("TargetingData", new TargetingData(PlotCourse(_gameStateHelper.GetVar<ActorEntity>("MainEntity"), _startingCoords, _targetedCoords), _targetedCoords));
    }

    private List<Vector2i> PlotCourse(Entity entity, Vector2i start, Vector2i end)
    {
      var path = new List<Vector2i>();
      var xDiff = end.X - start.X;
      var yDiff = end.Y - start.Y;
      var slope = 0D;
      var range = ((Weapon)entity.GetComponent<InventoryComponent>().GetActiveItem()).Range;

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
        if (xDiff >= 0)
        {
          for (int x = 0; x <= xDiff; x++)
          {
            var y = (int)Math.Round((double)x * slope);

            path.Add(new Vector2i(start.X + x, start.Y + y));

            if (TargetingShouldStop(start.X + x, start.Y + y, x, range)) break;
          }
        }
        else
        {
          for (int x = 0; x >= xDiff; x--)
          {
            var y = (int)Math.Round((double)x * slope);

            path.Add(new Vector2i(start.X + x, start.Y + y));

            if (TargetingShouldStop(start.X + x, start.Y + y, x, range)) break;
          }
        }
      }
      else
      {
        if (yDiff >= 0)
        {
          for (int y = 0; y <= yDiff; y++)
          {
            var x = (int)Math.Round((double)y / slope);

            path.Add(new Vector2i(start.X + x, start.Y + y));

            if (TargetingShouldStop(start.X + x, start.Y + y, y, range)) break;
          }
        }
        else
        {
          for (int y = 0; y >= yDiff; y--)
          {
            var x = (int)Math.Round((double)y / slope);

            path.Add(new Vector2i(start.X + x, start.Y + y));

            if (TargetingShouldStop(start.X + x, start.Y + y, y, range)) break;
          }
        }
      }

      return path;
    }

    // TODO: add other entities in the way to this check
    private bool TargetingShouldStop(int x, int y, int distance, int weaponRange)
    {
      // Targeting hit obstacle
      if (x < 0 || y < 0 || x >= _gameTiles.GetLength(0) || y >= _gameTiles.GetLength(1)
        || _tileDictionary[_gameTiles[x, y].TileType].BlocksProjectiles)
        return true;
      else if (Math.Abs(distance) >= weaponRange)
        return true;
      else
        return false;
    }

    private void PlayerRequestAttack()
    {
      var mainChar = _gameStateHelper.GetVar<ActorEntity>("MainEntity");

      SystemManager.BroadcastEvent(new EntityRequestAttackEvent(mainChar, _targetedCoords.X, _targetedCoords.Y));
    }

    /// <summary>
    /// When entity wants to attack, we need to check whether the attack is blocked in some way
    /// </summary>
    /// <param name="gameEvent"></param>
    private void EntityRequestAttack(EntityRequestAttackEvent gameEvent)
    {
      var entity = (Entity)gameEvent.EventParams[0];

      var invComp = entity.GetComponent<InventoryComponent>();
      var damage = 1;
      if (invComp != null)
      {
        var item = invComp.GetActiveItem();
        if (item != null)
        {
          var weapon = item as Weapon;
          if (weapon != null)
          {
            damage = weapon.Damage;
          }
        }
      }

      var targetX = (int)gameEvent.EventParams[1];
      var targetY = (int)gameEvent.EventParams[2];

      var entPos = entity.GetComponent<PositionComponent>();
      var startingX = entPos.ComponentXPositionOnMap;
      var startingY = entPos.ComponentYPositionOnMap;

      // Gives us true target co-ordinates, if path to target is obscured target will be the obstruction
      var path = PlotCourse(entity, new Vector2i(startingX, startingY), new Vector2i(targetX, targetY));
      var attackX = path[path.Count - 1].X;
      var attackY = path[path.Count - 1].Y;

      SystemManager.BroadcastEvent(new EntityAttackCoordsEvent(entity, damage, attackX, attackY));
    }
  }
}