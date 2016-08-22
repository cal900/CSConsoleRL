using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.GameSystems;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;
using CSConsoleRL.Components;

namespace CSConsoleRL.Game.Managers
{
    public class GameSystemManager
    {
        public event EventHandler WaitingForInput;
        public event EventHandler DoneWithInput;
        public List<Entity> Entities { get; set; }
        public Dictionary<Type, GameSystem> Systems { get; set; }

        public GameSystemManager()
        {
            Entities = new List<Entity>();
            Systems = new Dictionary<Type, GameSystem>();
        }

        public GameSystem RegisterSystem(GameSystem system)
        {
            Systems.Add(system.GetType(), system);
            system.SystemManager = this;
            return system;
        }

        public Entity RegisterEntity(Entity entity)
        {
            Entities.Add(entity);

            foreach(KeyValuePair<Type, GameSystem> system in Systems)
            {
                system.Value.AddEntity(entity);
            }

            return entity;
        }

        public void FireWaiting(object sender)
        {
            if (WaitingForInput != null) WaitingForInput(sender, new EventArgs());
        }

        public void FireDone(object sender)
        {
            if (DoneWithInput != null) DoneWithInput(sender, new EventArgs());
        }

        public void BroadcastUiEvent(/*GameEvent evt*/)
        {
            //if(_gameStateManager != null)_gameStateManager.CurrentState().HandleEvent(evt);
        }

        public void BroadcastEvent(GameEvent eventToBroadcast)
        {
            //foreach(GameSystem )
        }

        public void BroadcastEvent(GameEvent evnt, List<Entity> entitiesInvolved)
        {
            foreach(KeyValuePair<Type, GameSystem> system in Systems)
            {
                system.Value.BroadcastMessage(evnt, entitiesInvolved);
            }
        }

        public Entity WithId(Guid id)
        {
            return Entities.SingleOrDefault(e => e.Id == id);
        }
    }
}

//Systems should have list of Components
//Components have pointer to Entity, when broadcast event goes to the Entity owned by the Component
//E.g. for collision broadcast to all entities that have collision body, see if collision
//This way we can prepare if entity may have something related to collision but not directly collision component in future