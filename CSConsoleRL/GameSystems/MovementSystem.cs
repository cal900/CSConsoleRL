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

namespace CSConsoleRL.GameSystems
{
    public class MovementSystem : GameSystem
    {
        private List<Entity> collisionEntities;

        public MovementSystem(GameSystemManager manager)
        {
            SystemManager = manager;
            collisionEntities = new List<Entity>();
        }

        public override void AddComponent(IComponent component)
        {
            //collisionComponents.Add((component as MovementComponent));
        }

        public override GameEvent BroadcastMessage(GameEvent evnt)
        {
            if(evnt.EventName == "MovementInput")
            {
                HandleMovementInput(evnt);
            }
        }

        //If entity has MovementComponent check if can move to location (check collisions, if frozen etc.), otherwise don't anything
        public void HandleMovementInput(GameEvent evnt)
        {
            Guid entityId = (Guid)evnt.EventParams[0];
            Entity entityToMove = collisionEntities.Where(entity => entity.Id.Equals(entityId)).FirstOrDefault();

            int oldXPos = entityToMove.GetComponent<MovementComponent>().ComponentXPositionOnMap;
            int oldYPos = entityToMove.GetComponent<MovementComponent>().ComponentYPositionOnMap;
            int newXPos = oldXPos, newYPos = oldYPos;

            EnumDirections movementDirection = (EnumDirections)evnt.EventParams[1];

            switch(movementDirection)
            {
                case EnumDirections.North:
                    newYPos--;
                    break;
                case EnumDirections.South:
                    newYPos++;
                    break;
                case EnumDirections.East:
                    newXPos++;
                    break;
                case EnumDirections.West:
                    newXPos--;
                    break;
            }

            if (entityToMove == null) return;   //If system does not contain entity involved do nothing

            //iterate through all entities with collision component, check if collision
            foreach(Entity colEnt in collisionEntities)
            {

            }
        }
    }
}
