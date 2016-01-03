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
        private List<MovementComponent> collisionComponents;

        public MovementSystem(GameSystemManager manager)
        {
            SystemManager = manager;
            collisionComponents = new List<MovementComponent>();
        }

        public void AddComponent(IComponent component)
        {
            collisionComponents.Add((component as MovementComponent));
        }

        public void HandleMessage(GameEvent gameEvent)
        {

        }

        //If entity has MovementComponent check if can move to location (check collisions, if frozen etc.), otherwise don't anything
        public void HandleMovementRequest(Entity entity, EnumDirections direction)
        {
            MovementComponent movementComponent = entity.GetComponent<MovementComponent>();
            if (movementComponent == null) return;


        }
    }
}
