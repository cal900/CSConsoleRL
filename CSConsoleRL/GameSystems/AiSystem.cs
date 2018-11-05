using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Game.Managers;
using CSConsoleRL.Components;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Events;
using CSConsoleRL.Enums;
using CSConsoleRL.Entities;
using SFML.Graphics;
using SFML.Window;
using GameTiles.Tiles;

namespace CSConsoleRL.GameSystems
{
    public class AiSystem : GameSystem
    {
        private List<Entity> _aiEntities;
        private Tile[,] _gameTiles;

        public AiSystem(GameSystemManager _manager, Tile[,] _gameTiles)
        {
            SystemManager = _manager;
            _aiEntities = new List<Entity>();
            this._gameTiles = _gameTiles;
        }

        public override void InitializeSystem()
        {

        }

        public override void AddEntity(Entity entity)
        {
            if (entity.Components.ContainsKey(typeof(AiComponent)))
            {
                _aiEntities.Add(entity);
            }
        }

        public override void HandleMessage(IGameEvent gameEvent)
        {
            switch (gameEvent.EventName)
            {
                case "NextTurn":
                    NextTurn();
                    break;
            }
        }

        private void NextTurn()
        {
            foreach (var entity in _aiEntities)
            {
                GetAiResponse(entity);
            }
        }

        private void SetImmediatePositionGoal(Entity ent)
        {

        }

        private void GetAiResponse(Entity ent)
        {
            if(ent.HasComponent<AiComponent>())
            {
                GetSeekerResponse(ent);
            }
        }

        private void GetSeekerResponse(Entity ent)
        {
            
        }
    }
}
