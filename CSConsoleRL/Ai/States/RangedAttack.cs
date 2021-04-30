using CSConsoleRL.Ai.Interfaces;
using CSConsoleRL.Helpers;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;
using System.Collections.Generic;
using SFML.System;
using CSConsoleRL.Components;
using GameTiles.Tiles;

namespace CSConsoleRL.Ai.States
{
  public class RangedAttack : IAiState
  {
    private readonly Entity _entity;
    private int _counter;
    private List<Vector2i> _path;

    public RangedAttack(Entity entity)
    {
      _entity = entity;
      _path = new List<Vector2i>();
    }

    public string GetName()
    {
      return "MeleeSeek";
    }

    public IGameEvent GetAiStateResponse(GameStateHelper gameStateHelper)
    {
      var map = gameStateHelper.GetVar<Tile[,]>("GameTiles");
      var position = _entity.GetComponent<PositionComponent>();
      var mainChar = gameStateHelper.GetVar<Entity>("MainEntity");
      int horMovement = 0, verMovement = 0;
      int mainCharX = mainChar.GetComponent<PositionComponent>().ComponentXPositionOnMap;
      int mainCharY = mainChar.GetComponent<PositionComponent>().ComponentYPositionOnMap;

      //Call to A* Pathfinding to get path
      var path = PathfindingHelper.Instance.Path(map, new Vector2i(position.ComponentXPositionOnMap, position.ComponentYPositionOnMap),
          new Vector2i(mainCharX, mainCharY));

      if (path != null)
      {
        if (path != null) _path = path;
        //if (_path[1] != null) seekerAi.AnalyzedPath = path[1];

        // For debugging color in path and analyzed tiles
        // foreach (var tile in _path[1])
        //{
        //  var fadingColorEnt = new FadingColorEntity("yellow");
        //  fadingColorEnt.GetComponent<PositionComponent>().ComponentXPositionOnMap = tile.X;
        //  fadingColorEnt.GetComponent<PositionComponent>().ComponentYPositionOnMap = tile.Y;
        //  SystemManager.RegisterEntity(fadingColorEnt);
        //}
        //foreach (var tile in path)
        //{
        //  var fadingColorEnt = new FadingColorEntity("green");
        //  fadingColorEnt.GetComponent<PositionComponent>().ComponentXPositionOnMap = tile.X;
        //  fadingColorEnt.GetComponent<PositionComponent>().ComponentYPositionOnMap = tile.Y;
        //  SystemManager.RegisterEntity(fadingColorEnt);
        //}
      }

      //Move to desired location
      if (position.ComponentXPositionOnMap < mainCharX)
      {
        horMovement++;
      }
      else if (position.ComponentXPositionOnMap > mainCharX)
      {
        horMovement--;
      }
      if (position.ComponentYPositionOnMap < mainCharY)
      {
        verMovement++;
      }
      else if (position.ComponentYPositionOnMap > mainCharY)
      {
        verMovement--;
      }

      return new MovementInputEvent(_entity.Id, horMovement, verMovement);
    }
  }
}