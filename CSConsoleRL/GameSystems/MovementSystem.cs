using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Game.Managers;
using CSConsoleRL.Components;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Events;
using CSConsoleRL.Entities;
using CSConsoleRL.Enums;
using GameTiles.Tiles;
using CSConsoleRL.Helpers;

namespace CSConsoleRL.GameSystems
{
  public class MovementSystem : GameSystem
  {
    private readonly Tile[,] _gameTiles;
    private readonly TileTypeDictionary _tileDictionary;
    private readonly GameStateHelper _gameState;

    public MovementSystem(GameSystemManager manager, Tile[,] gameTiles, GameStateHelper gameState)
    {
      SystemManager = manager;
      _systemEntities = new List<Entity>();
      _gameTiles = gameTiles;
      _tileDictionary = new TileTypeDictionary();
      _gameState = gameState;
    }

    public override void InitializeSystem()
    {

    }

    public override void AddEntity(Entity entity)
    {
      if (entity.Components.ContainsKey(typeof(PositionComponent)) && entity.Components.ContainsKey(typeof(CollisionComponent)))
      {
        _systemEntities.Add(entity);
      }
    }

    public override void HandleMessage(IGameEvent gameEvent)
    {
      switch (gameEvent.EventName)
      {
        case "MovementInput":
          HandleMovementInput(gameEvent);
          break;
      }
    }

    //If entity has MovementComponent check if can move to location (check collisions, if frozen etc.), otherwise don't anything
    public void HandleMovementInput(IGameEvent evnt)
    {
      var movementEvent = (MovementInputEvent)evnt;

      Guid entityId = (Guid)evnt.EventParams[0];
      Entity entityToMove = _systemEntities.Where(entity => entity.Id.Equals(entityId)).FirstOrDefault();

      if (entityToMove == null) return;   //If system does not contain entity involved do nothing

      int desiredXPos = entityToMove.GetComponent<PositionComponent>().ComponentXPositionOnMap + (int)movementEvent.EventParams[1];
      int desiredYPos = entityToMove.GetComponent<PositionComponent>().ComponentYPositionOnMap + (int)movementEvent.EventParams[2];

      //If movement would carry entity out of map just return
      if (desiredYPos < 0 || desiredXPos < 0 || desiredYPos > _gameTiles.GetLength(1) - 1 || desiredXPos > _gameTiles.GetLength(0) - 1) return;

      //iterate through all entities with collision component, check if collision
      List<Entity> collisionEntities = _systemEntities.Where(ent => ent.Components.ContainsKey(typeof(CollisionComponent))).ToList();

      //check collision with other entities
      foreach (var colEnt in collisionEntities)
      {
        if (colEnt.Id != entityId
            && (colEnt.GetComponent<PositionComponent>().ComponentXPositionOnMap == desiredXPos
            && colEnt.GetComponent<PositionComponent>().ComponentYPositionOnMap == desiredYPos))
        {
          return;
        }
      }

      //check collision with impassible tiles
      if (_tileDictionary[_gameTiles[desiredXPos, desiredYPos].TileType].BlocksMovement) return;

      //passed all checks, move component
      entityToMove.GetComponent<PositionComponent>().ComponentXPositionOnMap = desiredXPos;
      entityToMove.GetComponent<PositionComponent>().ComponentYPositionOnMap = desiredYPos;
    }
  }
}
