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
        private Tile[,] _gameTiles;
        private ActorEntity _actorEntity;

        public AiSystem(GameSystemManager _manager, Tile[,] _gameTiles)
        {
            SystemManager = _manager;
            _systemEntities = new List<Entity>();
            this._gameTiles = _gameTiles;
        }

        public override void InitializeSystem()
        {

        }

        public override void AddEntity(Entity entity)
        {
            if (entity.GetType() == typeof(ActorEntity))
            {
                _actorEntity = (ActorEntity)entity;
            }
            if (entity.Components.ContainsKey(typeof(AiComponent)))
            {
                _systemEntities.Add(entity);
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
            foreach (var entity in _systemEntities)
            {
                GetAiResponse(entity);
            }
        }

        private void SetImmediatePositionGoal(Entity ent)
        {

        }

        private void GetAiResponse(Entity ent)
        {
            if (ent.HasComponent<AiComponent>())
            {
                GetSeekerResponse(ent);
            }
        }

        private void GetSeekerResponse(Entity ent)
        {
            var seekerAi = ent.GetComponent<SeekerAiComponent>();
            var position = ent.GetComponent<PositionComponent>();
            int horMovement = 0, verMovement = 0;

            //Set Seeker's goal location
            seekerAi.DesiredX = _actorEntity.GetComponent<PositionComponent>().ComponentXPositionOnMap + 2;
            seekerAi.DesiredY = _actorEntity.GetComponent<PositionComponent>().ComponentYPositionOnMap;

            //Move to desired location
            if (position.ComponentXPositionOnMap < seekerAi.DesiredX)
            {
                horMovement++;
            }
            else if (position.ComponentXPositionOnMap > seekerAi.DesiredX)
            {
                horMovement--;
            }
            if (position.ComponentYPositionOnMap < seekerAi.DesiredY)
            {
                verMovement++;
            }
            else if (position.ComponentYPositionOnMap > seekerAi.DesiredY)
            {
                verMovement--;
            }


        }
    }
}
