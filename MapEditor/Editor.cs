using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameTiles;
using GameTiles.Tiles;
using GameTiles.Enums;
using Utilities;

namespace MapEditor
{
    class MapEditor
    {
        static void Main(string[] args)
        {
            var Editor = new MapEditor();
        }

        public MapFile mapInfo;
        public TileTypeDictionary TileDictionary;
        public int CursorXPosition;
        public int CursorYPosition;
        public int WindowXPosition;
        public int WindowYPosition;
        public const int ScreenWidth = 20;
        public const int ScreenHeight = 20;

        public MapEditor()
        {
            CursorXPosition = 0;
            CursorYPosition = 0;
            WindowXPosition = 0;
            WindowYPosition = 0;

            mapInfo = new MapFile(new Tile[30, 30]);
            for (int y = 0; y < mapInfo.TileSet.GetLength(1); y++)
            {
                for (int x = 0; x < mapInfo.TileSet.GetLength(0); x++)
                {
                    mapInfo.TileSet[x, y].TileType = EnumTileTypes.Snow;
                }
            }

            TileDictionary = new TileTypeDictionary();

            MainLoop();
        }

        public void MainLoop()
        {
            DrawMap();
            ConsoleKeyInfo keyPressed;
            keyPressed = Console.ReadKey(false);
            while (keyPressed.Key != ConsoleKey.Escape)
            {
                if (keyPressed.Key == ConsoleKey.W)
                {
                    WindowYPosition--;
                }
                else if (keyPressed.Key == ConsoleKey.S)
                {
                    WindowYPosition++;
                }
                else if (keyPressed.Key == ConsoleKey.A)
                {
                    WindowXPosition--;
                }
                else if (keyPressed.Key == ConsoleKey.D)
                {
                    WindowXPosition++;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    if (CursorYPosition > 0) CursorYPosition--;
                    if (CursorYPosition < WindowYPosition) WindowYPosition--;
                }
                else if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    if (CursorYPosition < mapInfo.TileSet.GetLength(1) - 1) CursorYPosition++;
                    if (CursorYPosition >= WindowYPosition + ScreenWidth) WindowYPosition++;
                }
                else if (keyPressed.Key == ConsoleKey.LeftArrow)
                {
                    if (CursorXPosition > 0) CursorXPosition--;
                    if (CursorXPosition < WindowXPosition) WindowXPosition--;
                }
                else if (keyPressed.Key == ConsoleKey.RightArrow)
                {
                    if (CursorXPosition < mapInfo.TileSet.GetLength(0) - 1) CursorXPosition++;
                    if (CursorXPosition >= WindowXPosition + ScreenWidth) WindowXPosition++;
                }
                else if (keyPressed.Key == ConsoleKey.M)
                {
                    var newFileMenu = new MapFileHandler(ref mapInfo);
                }
                else
                {
                    HandleTileChangeKeyPressed(keyPressed.Key);
                }

                DrawMap();

                keyPressed = Console.ReadKey(false);
            }
        }

        public void DrawMap()
        {
            Console.Clear();

            for (int y = WindowYPosition; y < WindowYPosition + ScreenHeight; y++)
            {
                for (int x = WindowXPosition; x < WindowXPosition + ScreenWidth; x++)
                {
                    //If the position is out of bounds for the map don't draw anything there
                    if (x < 0 || y < 0 || x >= mapInfo.TileSet.GetLength(0) || y >= mapInfo.TileSet.GetLength(1))
                    {
                        Console.Write("  "); 
                        continue;
                    }

                    Console.ForegroundColor = TileDictionary[mapInfo.TileSet[x, y].TileType].Color;
                    if (x == CursorXPosition && y == CursorYPosition) Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(TileDictionary[mapInfo.TileSet[x, y].TileType].Character + " ");
                }
                Console.WriteLine("");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\nCursorX\t" + CursorXPosition + "\nCursorY\t" + CursorYPosition + "\nWindowX\t" + WindowXPosition + "\nWindowY\t" + WindowYPosition);
        }
        public void HandleTileChangeKeyPressed(ConsoleKey keyPressed)
        {
            if (keyPressed == ConsoleKey.D1)
            {
                mapInfo.TileSet[CursorXPosition, CursorYPosition].TileType = EnumTileTypes.Snow;
            }
            else if (keyPressed == ConsoleKey.D2)
            {
                mapInfo.TileSet[CursorXPosition, CursorYPosition].TileType = EnumTileTypes.SnowWalked;
            }
            else if (keyPressed == ConsoleKey.D3)
            {
                mapInfo.TileSet[CursorXPosition, CursorYPosition].TileType = EnumTileTypes.Road;
            }
            else if (keyPressed == ConsoleKey.D4)
            {
                mapInfo.TileSet[CursorXPosition, CursorYPosition].TileType = EnumTileTypes.Grass;
            }
            else if (keyPressed == ConsoleKey.D5)
            {
                mapInfo.TileSet[CursorXPosition, CursorYPosition].TileType = EnumTileTypes.CabinFloor;
            }
            else if (keyPressed == ConsoleKey.D6)
            {
                mapInfo.TileSet[CursorXPosition, CursorYPosition].TileType = EnumTileTypes.CabinWall;
            }
            else if (keyPressed == ConsoleKey.D7)
            {
                mapInfo.TileSet[CursorXPosition, CursorYPosition].TileType = EnumTileTypes.CabinDoor;
            }
            else if (keyPressed == ConsoleKey.D8)
            {
                mapInfo.TileSet[CursorXPosition, CursorYPosition].TileType = EnumTileTypes.Tree;
            }
            else if (keyPressed == ConsoleKey.D9)
            {
                mapInfo.TileSet[CursorXPosition, CursorYPosition].TileType = EnumTileTypes.River;
            }
            else if (keyPressed == ConsoleKey.D0)
            {
                mapInfo.TileSet[CursorXPosition, CursorYPosition].TileType = EnumTileTypes.CabinWindow;
            }
        }
    }
}
