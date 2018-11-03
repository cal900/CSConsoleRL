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
using GameTiles.Enums;
using GameTiles.Tiles;

namespace CSConsoleRL.GameSystems
{
    public class LosSystem : GameSystem
    {
        private List<Entity> _losEntities;
        private Tile[,] _gameTiles;
        private TileTypeDictionary _tileDictionary;
        private bool _fowEnabled;
        public Entity LosSourceEntity { get; set; }

        public LosSystem(GameSystemManager manager, Tile[,] gameTiles)
        {
            SystemManager = manager;
            _losEntities = new List<Entity>();
            _gameTiles = gameTiles;
            _tileDictionary = new TileTypeDictionary();
            _fowEnabled = false;
        }

        public override void InitializeSystem()
        {

        }

        public override void AddEntity(Entity entity)
        {
            if (entity.Components.ContainsKey(typeof(PositionComponent)) && entity.Components.ContainsKey(typeof(LosComponent)))
            {
                _losEntities.Add(entity);
            }
        }

        public override void HandleMessage(IGameEvent gameEvent)
        {
            switch (gameEvent.EventName)
            {
                case "NextFrame":
                    //CalculateLos();
                    break;
                case "NextTurn":
                    CalculateLos();
                    break;
                case "ToggleFow":
                    _fowEnabled = !_fowEnabled;
                    break;
            }
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

        //Have starting and end slope, then just need if y is negative or positive and can figure out quadrant from that
        //If sSlope is 1 or |sSlope| > 1 Y oriented scan

        private void CalculateLos()
        {
            //Set all LOS to false
            //this sets entire map to false (maybe a waste? 
            for (int y = 0; y < _gameTiles.GetLength(1); y++)
            {
                for (int x = 0; x < _gameTiles.GetLength(0); x++)
                {
                    _gameTiles[x, y].IsInLos = !_fowEnabled;
                }
            }

            if (_fowEnabled && LosSourceEntity != null)
            {
                ScanLos(5, LosSourceEntity.GetComponent<PositionComponent>().ComponentXPositionOnMap, LosSourceEntity.GetComponent<PositionComponent>().ComponentYPositionOnMap);
            }
        }

        private void ScanLos(int sightRange, int startX, int startY)
        {
            //Figure out square range need to calculate
            //Top left first

        }

        private void RayTrace(int startX, int startY, int endX, int endY)
        {
            //Make sure we aren't outside the map bounds
            if (startX < 0) startX = 0;
            else if (startX >= _gameTiles.GetLength(0)) startX = _gameTiles.GetLength(0) - 1;
            if (endX < 0) endX = 0;
            else if (endX >= _gameTiles.GetLength(0)) endX = _gameTiles.GetLength(0) - 1;
            if (startY < 0) startY = 0;
            else if (startY >= _gameTiles.GetLength(0)) startY = _gameTiles.GetLength(0) - 1;
            if (endY < 0) endY = 0;
            else if (endY >= _gameTiles.GetLength(0)) endY = _gameTiles.GetLength(0) - 1;

            decimal slope = (endY - startY) / (endX - startX);

        }
    }
}
