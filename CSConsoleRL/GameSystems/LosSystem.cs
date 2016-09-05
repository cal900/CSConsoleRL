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
        public Entity LosSourceEntity { get; set; }

        public LosSystem(GameSystemManager manager, Tile[,] _gameTiles)
        {
            SystemManager = manager;
            losEntities = new List<Entity>();
            gameTiles = _gameTiles;
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

          //           Shared
          //           edge by
          //Shared     1 & 2      Shared
          //edge by\      |      /edge by
          //1 & 8   \     |     / 2 & 3
          //         \1111|2222/
          //         8\111|222/3
          //         88\11|22/33
          //         888\1|2/333
          //Shared   8888\|/3333  Shared
          //edge by-------@-------edge by
          //7 & 8    7777/|\4444  3 & 4
          //         777/6|5\444
          //         77/66|55\44
          //         7/666|555\4
          //         /6666|5555\
          //Shared  /     |     \ Shared
          //edge by/      |      \edge by
          //6 & 7      Shared     4 & 5
          //           edge by 
          //           5 & 6

        private void CalculateLos()
        {
            if (LosSourceEntity != null)
            {

            }
        }

        private void ScanQuadrant(int startingY, int startingX)
        {
            int rowXStart = startingX;
            for(int y = startingY; y > 0; y--)
            {
                for(int x = rowXStart; x <= startingX; x++)
                {

                }
                rowXStart--;
            }
        }
    }
}
