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
        private List<Entity> movementEntities;

        public MovementSystem(GameSystemManager manager)
        {
            SystemManager = manager;
            movementEntities = new List<Entity>();
        }

        public override void AddEntity(Entity entity)
        {
            if(entity.Components.ContainsKey(typeof(PositionComponent)) && entity.Components.ContainsKey(typeof(CollisionComponent)))
            {
                movementEntities.Add(entity);
            }
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

            if (entityToMove == null) return;   //If system does not contain entity involved do nothing

            //iterate through all entities with collision component, check if collision
            List<CollisionComponent> collisionComponents = collisionEntities.Where(currentEntity => currentEntity.Components.ContainsKey(typeof(CollisionComponent))).ToList();
        }
    }
}
