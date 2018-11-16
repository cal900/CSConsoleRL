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
using SFML.System;
using GameTiles.Tiles;
using CSConsoleRL.Helpers;

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
            if (entity.Components.ContainsKey(typeof(AiComponent))
                || entity.Components.ContainsKey(typeof(SeekerAiComponent)))
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

            GetSeekerResponse(ent);
        }

        private void GetSeekerResponse(Entity ent)
        {
            var seekerAi = ent.GetComponent<SeekerAiComponent>();
            var position = ent.GetComponent<PositionComponent>();
            int horMovement = 0, verMovement = 0;

            //Set Seeker's goal location
            seekerAi.DesiredX = _actorEntity.GetComponent<PositionComponent>().ComponentXPositionOnMap + 2;
            seekerAi.DesiredY = _actorEntity.GetComponent<PositionComponent>().ComponentYPositionOnMap;

            //Call to A* Pathfinding to get path
            var path = PathfindingHelper.Instance.Path(_gameTiles, new Vector2i(position.ComponentXPositionOnMap, position.ComponentYPositionOnMap),
                new Vector2i(seekerAi.DesiredX, seekerAi.DesiredY));

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

            BroadcastMessage(new MovementInputEvent(ent.Id, horMovement, verMovement));
        }
    }
}
