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

namespace CSConsoleRL.GameSystems
{
    public class MovementSystem : GameSystem
    {
        private List<Entity> movementEntities;
        private Tile[,] gameTiles;
        private TileTypeDictionary tileDictionary;

        public MovementSystem(GameSystemManager manager, Tile[,] _gameTiles)
        {
            SystemManager = manager;
            movementEntities = new List<Entity>();
            gameTiles = _gameTiles;
            tileDictionary = new TileTypeDictionary();
        }

        public override void InitializeSystem()
        {
            throw new NotImplementedException();
        }

        public override void AddEntity(Entity entity)
        {
            if (entity.Components.ContainsKey(typeof(PositionComponent)) && entity.Components.ContainsKey(typeof(CollisionComponent)))
            {
                movementEntities.Add(entity);
            }
        }

        public override void HandleMessage(GameEvent gameEvent)
        {
            switch (gameEvent.EventName)
            {
                case "MovementInput":
                    HandleMovementInput(gameEvent);
                    break;
            }
        }

        public override GameEvent BroadcastMessage(GameEvent evnt)
        {
            throw new NotImplementedException();
        }

        //If entity has MovementComponent check if can move to location (check collisions, if frozen etc.), otherwise don't anything
        public void HandleMovementInput(GameEvent evnt)
        {
            var movementEvent = (MovementEvent)evnt;

            Guid entityId = (Guid)evnt.EventParams[0];
            Entity entityToMove = movementEntities.Where(entity => entity.Id.Equals(entityId)).FirstOrDefault();

            if (entityToMove == null) return;   //If system does not contain entity involved do nothing

            int desiredXPos = entityToMove.GetComponent<PositionComponent>().ComponentXPositionOnMap;
            int desiredYPos = entityToMove.GetComponent<PositionComponent>().ComponentYPositionOnMap;

            var movementDirection = (EnumDirections)movementEvent.EventParams[1];

            switch (movementDirection)
            {
                case EnumDirections.North:
                    desiredYPos--;
                    break;
                case EnumDirections.South:
                    desiredYPos++;
                    break;
                case EnumDirections.West:
                    desiredXPos--;
                    break;
                case EnumDirections.East:
                    desiredXPos++;
                    break;
            }

            //If movement would carry entity out of map just return
            if (desiredYPos < 0 || desiredXPos < 0 || desiredYPos > gameTiles.GetLength(1) || desiredXPos > gameTiles.GetLength(0)) return;

            //iterate through all entities with collision component, check if collision
            List<Entity> collisionEntities = movementEntities.Where(ent => ent.Components.ContainsKey(typeof(CollisionComponent))).ToList();

            //check collision with other entities
            foreach (var colEnt in collisionEntities)
            {
                if (colEnt.GetComponent<PositionComponent>().ComponentXPositionOnMap == desiredXPos
                    || colEnt.GetComponent<PositionComponent>().ComponentYPositionOnMap == desiredYPos)
                {
                    return;
                }
            }

            //check collision with impassible tiles
            if (tileDictionary[gameTiles[desiredXPos, desiredYPos].TileType].BlocksMovement) return;

            //passed all checks, move component
            entityToMove.GetComponent<PositionComponent>().ComponentXPositionOnMap = desiredXPos;
            entityToMove.GetComponent<PositionComponent>().ComponentYPositionOnMap = desiredYPos;
        }
    }
}
