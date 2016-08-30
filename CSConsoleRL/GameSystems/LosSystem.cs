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
    public class LosSystem : GameSystem
    {
        private List<Entity> losEntities;
        private Tile[,] gameTiles;
        private Entity losSource;

        public LosSystem(GameSystemManager manager, Tile[,] _gameTiles, Entity _losSource)
        {
            SystemManager = manager;
            losEntities = new List<Entity>();
            gameTiles = _gameTiles;
            losSource = _losSource;
        }

        public override void InitializeSystem()
        {
            
        }

        public override void AddEntity(Entity entity)
        {
            if (entity.Components.ContainsKey(typeof(PositionComponent)) && entity.Components.ContainsKey(typeof(LosComponent)))
            {
                losEntities.Add(entity);
            }
        }

        public override void HandleMessage(IGameEvent gameEvent)
        {
            switch (gameEvent.EventName)
            {
                case "NextFrame":
                    CalculateLos();
                    break;
            }
        }

        public override void BroadcastMessage(IGameEvent evnt)
        {
            throw new NotImplementedException();
        }

        public void CalculateLos()
        {
            
        }
    }
}
