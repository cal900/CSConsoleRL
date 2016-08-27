using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.GameSystems;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;
using CSConsoleRL.Components;
using SFML.Window;
using SFML.Graphics;
using GameTiles.Tiles;

namespace CSConsoleRL.Game.Managers
{
    public class GameSystemManager
    {
        public event EventHandler WaitingForInput;
        public event EventHandler DoneWithInput;
        public List<Entity> Entities { get; set; }
        public Dictionary<Type, GameSystem> Systems { get; set; }
        private RenderWindow sfmlWindow { get; set; }
        private Tile[,] gameTiles { get; set; }

        public GameSystemManager()
        {
            Entities = new List<Entity>();
            Systems = new Dictionary<Type, GameSystem>();

            sfmlWindow = new RenderWindow(new VideoMode(720, 480), "CSConsoleRL");

            Initial
        }

        private void InitializeSystems()
        {
            MovementSystem = new MovementSystem()
        }

        private GameSystem RegisterSystem(GameSystem system)
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

        public void BroadcastEvent(GameEvent evnt)
        {
            foreach(KeyValuePair<Type, GameSystem> system in Systems)
            {
                system.Value.BroadcastMessage(evnt);
            }
        }

        public Entity WithId(Guid id)
        {
            return Entities.SingleOrDefault(e => e.Id == id);
        }
    }
}