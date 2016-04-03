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
    public class CollisionSystem : GameSystem
    {
        private List<CollisionComponent> collisionComponents;

        public CollisionSystem(GameSystemManager manager)
        {
            SystemManager = manager;
            collisionComponents = new List<MovementComponent>();
        }

        public override void AddComponent(IComponent component)
        {
            collisionComponents.Add((component as MovementComponent));
        }

        public override GameEvent BroadcastMessage(GameEvent evnt, List<Entity> entitiesInvolved)
        {
            if(evnt.EventName == "MovementInput")
            {
                HandleMovementInput(evnt, entitiesInvolved);
            }
        }

        //If entity has MovementComponent check if can move to location (check collisions, if frozen etc.), otherwise don't anything
        public void HandleMovementInput(GameEvent evnt, List<Entity> entitiesInvolved)
        {
            
        }
    }
}
