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
        private List<Entity> losEntities;
        private Tile[,] gameTiles;
        public Entity LosSourceEntity { get; set; }
        private TileTypeDictionary tileDictionary;
        private bool _fowEnabled;

        public LosSystem(GameSystemManager manager, Tile[,] _gameTiles)
        {
            SystemManager = manager;
            losEntities = new List<Entity>();
            gameTiles = _gameTiles;
            tileDictionary = new TileTypeDictionary();
            _fowEnabled = false;
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
            for (int y = 0; y < gameTiles.GetLength(1); y++)
            {
                for (int x = 0; x < gameTiles.GetLength(0); x++)
                {
                    gameTiles[x, y].IsInLos = !_fowEnabled;
                }
            }

            if (_fowEnabled && LosSourceEntity != null)
            {
                //Quad 1 & 2
                //ScanQuadrant(LosSourceEntity.GetComponent<PositionComponent>().ComponentYPositionOnMap, LosSourceEntity.GetComponent<PositionComponent>().ComponentXPositionOnMap, 0, 1, double.MaxValue, false);
                //Quad 5
                //ScanQuadrant(LosSourceEntity.GetComponent<PositionComponent>().ComponentYPositionOnMap, LosSourceEntity.GetComponent<PositionComponent>().ComponentXPositionOnMap, 0, double.MaxValue, -1, true);
                //Quad 6
                ScanQuadrant(LosSourceEntity.GetComponent<PositionComponent>().ComponentYPositionOnMap, LosSourceEntity.GetComponent<PositionComponent>().ComponentXPositionOnMap, 0, 1, double.MaxValue, true);
                ScanQuadrant(LosSourceEntity.GetComponent<PositionComponent>().ComponentYPositionOnMap, LosSourceEntity.GetComponent<PositionComponent>().ComponentXPositionOnMap, 0, double.MaxValue, -1, true);
                ScanQuadrant(LosSourceEntity.GetComponent<PositionComponent>().ComponentYPositionOnMap, LosSourceEntity.GetComponent<PositionComponent>().ComponentXPositionOnMap, 0, -1, double.MinValue, false);
            }
        }

        //will need seperate case for each scenario as needs to branch outward, otherwise doesn't work on edges (e.g. for bottom left sector, gets killed on edge as stops when x < 0)
        private void ScanQuadrant(int absoluteY, int absoluteX, int yOffset, double startSlope, double endSlope, bool yPositive)
        {
            //Y-Positive oriented scan
            if (yPositive)
            {
                for (int y = absoluteY + yOffset; y < 30; y++)
                {
                    //something going on causing a scan that starts at place, marks it as in LOS so we see it, then the next is false so stops (causing random visible squares) - slopes?

                    //Find out starting and ending X values for this row
                    //Converting to int immediately was causing some unexpected tiles to be hidden, rounding down for startX and rounding up for endX gives more intuitive LOS
                    int rowXStartOffset = 0;
                    double xStartDouble = (y - absoluteY) / endSlope;
                    if (Math.Abs(xStartDouble) > 0.5d) rowXStartOffset = Convert.ToInt32(Math.Floor(xStartDouble));
                    int rowXEndOffset = 0;
                    double xEndDouble = (y - absoluteY) / startSlope;
                    if (Math.Abs(xEndDouble) > 0.5d) rowXEndOffset = Convert.ToInt32(Math.Ceiling(xEndDouble));
                    for (int x = absoluteX + rowXStartOffset; (x <= absoluteX + rowXEndOffset && x < 30 && x >= 0); x++)
                    {
                        gameTiles[x, y].IsInLos = true;

                        if (tileDictionary[gameTiles[x, y].TileType].BlocksVision)
                        {
                            //Split the remaining LOS calculation in this quadrant into two, if there are further vision blocking tiles will be caught in the recursive call
                            //First need calculate end slope of the first scan (start slope is same)
                            //X - 1 because we want location just to the left of blocker for the first scan
                            double recStartSlope = Convert.ToDouble(x - absoluteX - 1) == 0 ? double.MaxValue : Convert.ToDouble(y - absoluteY) / Convert.ToDouble(x - absoluteX - 1);
                            //if (recStartSlope > 0 && endSlope >= recStartSlope) 
                                ScanQuadrant(absoluteY, absoluteX, (y - absoluteY), recStartSlope, endSlope, true);

                            //Find where LOS blocker ends and start scan from there
                            int secondScanX = x;

                            while (tileDictionary[gameTiles[secondScanX, y].TileType].BlocksVision)
                            {
                                if (secondScanX <= absoluteX + Convert.ToInt32((y - absoluteY) / startSlope) && secondScanX < 30)
                                {
                                    gameTiles[secondScanX, y].IsInLos = true;
                                    secondScanX++;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            double recEndSlope = Convert.ToDouble(y - absoluteY) / Convert.ToDouble(secondScanX - absoluteX);
                            //if (recEndSlope > 0 && recEndSlope >= startSlope) 
                                ScanQuadrant(absoluteY, absoluteX, (y - absoluteY), startSlope, recEndSlope, true);

                            return;
                        }
                    }
                }
            }
            //Y-Negative oriented scan
            else
            {
                for (int y = absoluteY + yOffset; y >= 0; y--)
                {
                    //something going on causing a scan that starts at place, marks it as in LOS so we see it, then the next is false so stops (causing random visible squares) - slopes?

                    //Find out starting and ending X values for this row
                    int rowXStartOffset = 0;
                    double xStartDouble = (y - absoluteY) / endSlope;
                    if (Math.Abs(xStartDouble) > 0.5d) rowXStartOffset = Convert.ToInt32(Math.Floor(xStartDouble));
                    int rowXEndOffset = 0;
                    double xEndDouble = (y - absoluteY) / startSlope;
                    if (Math.Abs(xEndDouble) > 0.5d) rowXEndOffset = Convert.ToInt32(Math.Ceiling(xEndDouble));
                    for (int x = absoluteX + rowXStartOffset; (x <= absoluteX + rowXEndOffset && x < 30 && x >= 0); x++)
                    {
                        gameTiles[x, y].IsInLos = true;

                        if (tileDictionary[gameTiles[x, y].TileType].BlocksVision)
                        {
                            //Split the remaining LOS calculation in this quadrant into two, if there are further vision blocking tiles will be caught in the recursive call
                            //First need calculate end slope of the first scan (start slope is same)
                            //X - 1 because we want location just to the left of blocker for the first scan
                            double recStartSlope = Convert.ToDouble(x - absoluteX - 1) == 0 ? double.MinValue : Convert.ToDouble(y - absoluteY) / Convert.ToDouble(x - absoluteX - 1);
                            if (recStartSlope < 0 && endSlope <= recStartSlope) ScanQuadrant(absoluteY, absoluteX, (y - absoluteY), recStartSlope, endSlope, false);

                            //Find where LOS blocker ends and start scan from there
                            int secondScanX = x;

                            while (tileDictionary[gameTiles[secondScanX, y].TileType].BlocksVision)
                            {
                                if (secondScanX <= absoluteX + Convert.ToInt32((y - absoluteY) / startSlope) && secondScanX < 30)
                                {
                                    gameTiles[secondScanX, y].IsInLos = true;
                                    secondScanX++;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            double recEndSlope = Convert.ToDouble(y - absoluteY) / Convert.ToDouble(secondScanX - absoluteX);
                            if (recEndSlope < 0 && recEndSlope <= startSlope) ScanQuadrant(absoluteY, absoluteX, (y - absoluteY), startSlope, recEndSlope, false);

                            return;
                        }
                    }
                }
            }
        }
    }
}
