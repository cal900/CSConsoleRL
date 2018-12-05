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
using SFML.System;
using GameTiles.Tiles;
using GameTiles;

namespace CSConsoleRL.Game.Managers
{
    public class GameSystemManager
    {
        public event EventHandler WaitingForInput;
        public event EventHandler DoneWithInput;
        public List<Entity> Entities { get; set; }
        public Dictionary<Type, GameSystem> Systems { get; set; }
        private RenderWindow sfmlWindow { get; set; }
        private MapFile gameMap { get; set; }

        private bool exitGame;

        public GameSystemManager(MapFile _gameMap)
        {
            gameMap = _gameMap;
            Entities = new List<Entity>();
            Systems = new Dictionary<Type, GameSystem>();

            sfmlWindow = new RenderWindow(new VideoMode(600, 600), "CSConsoleRL");

            CreateSystems();

            CreateEntities();

            InitializeSystems();

            MainGameLoop();
        }

        private void CreateEntities()
        {
            var mainChar = new ActorEntity();
            RegisterEntity(mainChar);
            ((LosSystem)Systems[typeof(LosSystem)]).SystemEntities = mainChar;
        }

        private void CreateSystems()
        {
            RegisterSystem(new MovementSystem(this, gameMap.TileSet));
            RegisterSystem(new LosSystem(this, gameMap.TileSet));
            RegisterSystem(new SfmlGraphicsSystem(this, sfmlWindow, gameMap.TileSet));
            RegisterSystem(new UserInputSystem(this, sfmlWindow));
            RegisterSystem(new ConsoleSystem(this));
            RegisterSystem(new AiSystem(this, gameMap.TileSet));
        }

        private void InitializeSystems()
        {
            foreach (KeyValuePair<Type, GameSystem> system in Systems)
            {
                system.Value.InitializeSystem();
            }
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

            foreach (KeyValuePair<Type, GameSystem> system in Systems)
            {
                system.Value.AddEntity(entity);
            }

            return entity;
        }

        public List<Entity> GetEntities<T>()
        {
            var matchingEnts = new List<Entity>();
            foreach (var ent in Entities)
            {
                if(ent.GetType() == typeof(T))
                {
                    matchingEnts.Add(ent);
                }
            }

            return matchingEnts;
        }

        public void BroadcastUiEvent(/*GameEvent evt*/)
        {
            //if(_gameStateManager != null)_gameStateManager.CurrentState().HandleEvent(evt);
        }

        public void BroadcastEvent(IGameEvent evnt)
        {
            if (evnt.EventName == "ExitGame")
            {
                exitGame = true;
            }

            foreach (KeyValuePair<Type, GameSystem> system in Systems)
            {
                system.Value.HandleMessage(evnt);
            }
        }

        public Entity WithId(Guid id)
        {
            return Entities.SingleOrDefault(e => e.Id == id);
        }

        private void MainGameLoop()
        {
            //Clock clock = new Clock();
            sfmlWindow.SetFramerateLimit(30);
            sfmlWindow.SetVerticalSyncEnabled(false);

            var nextFrameEvent = new NextFrameEvent();

            while (sfmlWindow.IsOpen && !exitGame)
            {
                //var timeElapsed = clock.ElapsedTime;
                sfmlWindow.DispatchEvents();
                BroadcastEvent(nextFrameEvent);
            }
        }
    }
}