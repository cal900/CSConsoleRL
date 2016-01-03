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

            //If entity contains components governed by a system (graphics, user input etc), add the component to the relevant system
            if(entity.Components.ContainsKey(typeof(DrawableCharComponent)))
            {
                Systems[typeof(CharGraphicsSystem)].AddComponent(entity.Components[typeof(DrawableCharComponent)]);
            }
            if (entity.Components.ContainsKey(typeof(UserInputComponent)))
            {
                Systems[typeof(UserInputSystem)].AddComponent(entity.Components[typeof(UserInputComponent)]);
            }
            if (entity.Components.ContainsKey(typeof(MovementComponent)))
            {
                Systems[typeof(MovementSystem)].AddComponent(entity.Components[typeof(MovementComponent)]);
            }

            return entity;
        }

        public void FireWaiting(object sender)
        {
            if (WaitingForInput != null) WaitingForInput(sender, new EventArgs());
        }

        public void FireDone(object sender)
        {
            if(DoneWithInput != null) DoneWithInput(sender, new EventArgs());
        }

        public void FireUiEvent(/*GameEvent evt*/)
        {
            //if(_gameStateManager != null)_gameStateManager.CurrentState().HandleEvent(evt);
        }

        public void FireEvent(GameEvent evt)
        {
            //Systems.ForEach(s => s.HandleMessage(evt));
        }

        public Entity WithId(Guid id)
        {
            return Entities.SingleOrDefault(e => e.Id == id);
        }
    }
}
