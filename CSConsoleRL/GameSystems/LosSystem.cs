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
            //Need to set the initial state of all squares
            SetAllTiles();
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

        private void SetAllTiles()
        {
            for (int y = 0; y < _gameTiles.GetLength(1); y++)
            {
                for (int x = 0; x < _gameTiles.GetLength(0); x++)
                {
                    _gameTiles[x, y].IsInLos = !_fowEnabled;
                }
            }
        }

        private void CalculateLos()
        {
            SetAllTiles();

            if (_fowEnabled && LosSourceEntity != null)
            {
                ScanLos(10, LosSourceEntity.GetComponent<PositionComponent>().ComponentXPositionOnMap, LosSourceEntity.GetComponent<PositionComponent>().ComponentYPositionOnMap);
            }
        }

        private void ScanLos(int sightRange, int startX, int startY)
        {
            //Figure out square range need to calculate
            //First do top horizontal line
            int topLosY = startY - sightRange;
            if (topLosY < 0) topLosY = 0;
            for (int x = (startX - sightRange); x <= (startX + sightRange); x++)
            {
                RayTrace(sightRange, startX, startY, x, topLosY);
            }

            //Bottom horizontal line
            int botLosY = startY + sightRange;
            if (botLosY >= _gameTiles.GetLength(1)) botLosY = _gameTiles.GetLength(1) - 1;
            for (int x = (startX - sightRange); x <= (startX + sightRange); x++)
            {
                RayTrace(sightRange, startX, startY, x, botLosY);
            }

            //Left vertical line
            int leftLosX = startX - sightRange;
            if (leftLosX < 0) leftLosX = 0;
            for (int y = (startY - sightRange); y <= (startY + sightRange); y++)
            {
                RayTrace(sightRange, startX, startY, leftLosX, y);
            }

            //Right vertical line
            int rightLosX = startX + sightRange;
            if (rightLosX < 0) rightLosX = 0;
            for (int y = (startY - sightRange); y <= (startY + sightRange); y++)
            {
                RayTrace(sightRange, startX, startY, rightLosX, y);
            }
        }

        private void RayTrace(int sightRange, int startX, int startY, int endX, int endY)
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

            decimal increX = (decimal)(endX - startX) / sightRange;
            decimal increY = (decimal)(endY - startY) / sightRange;

            decimal relX = 0;
            decimal relY = 0;

            for(int i = 0; i < sightRange; i++)
            {
                relX = i * increX;
                relY = i * increY;

                int absX = Decimal.ToInt32(Decimal.Round(relX)) + startX;
                int absY = Decimal.ToInt32(Decimal.Round(relY)) + startY;

                _gameTiles[absX, absY].IsInLos = true;

                //If present tile blocks sight, stop the ray trace (unless tile is where the user is e.g. a door)
                if(i > 0 && _tileDictionary[_gameTiles[absX, absY].TileType].BlocksVision)
                {
                    return;
                }
            }
        }
    }
}
