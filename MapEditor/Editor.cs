using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameTiles;
using GameTiles.Tiles;
using GameTiles.Enums;
using GameTiles.Spawns;

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
    public const int ScreenWidth = 21;
    public const int ScreenHeight = 21;
    private int _cursorSize;

    public MapEditor()
    {
      CursorXPosition = 0;
      CursorYPosition = 0;
      WindowXPosition = 0;
      WindowYPosition = 0;
      _cursorSize = 1;

      mapInfo = new MapFile(new Tile[50, 50], new List<Spawn>());
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
      keyPressed = Console.ReadKey(true);
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
        else if (keyPressed.Key == ConsoleKey.Add)
        {
          if (_cursorSize < 2)
          {
            _cursorSize++;
          }
        }
        else if (keyPressed.Key == ConsoleKey.Subtract)
        {
          if (_cursorSize > 0)
          {
            _cursorSize--;
          }
        }
        else if (keyPressed.Key == ConsoleKey.B)
        {
          Console.Clear();
          // Create new map with specified 
          Console.WriteLine("Enter the new map width (in tiles)");
          var newX = Console.ReadLine();
          Console.WriteLine("Enter the new map height (in tiles)");
          var newY = Console.ReadLine();
          mapInfo = new MapFile(new Tile[int.Parse(newX), int.Parse(newY)], new List<Spawn>());
          ResetMap();
        }
        else if (keyPressed.Key == ConsoleKey.M)
        {
          var newFileMenu = new MapFileHandler.MapFileHandler(ref mapInfo);
        }
        else if (keyPressed.Key == ConsoleKey.N)
        {
          ResetMap();
        }
        else
        {
          HandleTileChangeKeyPressed(keyPressed.Key);
        }

        DrawMap();

        keyPressed = Console.ReadKey(true);
      }
    }

    public void ResetMap()
    {
      //Reset/Clear map to all snow
      for (int yIndex = 0; yIndex < mapInfo.TileSet.GetLength(1); yIndex++)
      {
        for (int xIndex = 0; xIndex < mapInfo.TileSet.GetLength(0); xIndex++)
        {
          mapInfo.TileSet[xIndex, yIndex].TileType = EnumTileTypes.Snow;
        }
      }
    }

    public void DrawMap()
    {
      Console.Clear();

      var windowYPosition = CursorYPosition - (ScreenHeight - 1) / 2;
      var windowXPosition = CursorXPosition - (ScreenWidth - 1) / 2;
      for (int y = windowYPosition; y < windowYPosition + ScreenHeight; y++)
      {
        for (int x = windowXPosition; x < windowXPosition + ScreenWidth; x++)
        {
          //If the position is out of bounds for the map don't draw anything there
          if (x < 0 || y < 0 || x >= mapInfo.TileSet.GetLength(0) || y >= mapInfo.TileSet.GetLength(1))
          {
            Console.Write("  ");
            continue;
          }

          Console.ForegroundColor = TileDictionary[mapInfo.TileSet[x, y].TileType].Color;
          if (IsCoordInCursor(x, y)) Console.ForegroundColor = ConsoleColor.Yellow;
          Console.Write(TileDictionary[mapInfo.TileSet[x, y].TileType].Character + " ");
        }
        Console.WriteLine("");
      }

      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine(mapInfo.TileSet[CursorXPosition, CursorYPosition].TileType);
      Console.WriteLine("CursorX\t" + CursorXPosition);
      Console.WriteLine("CursorY\t" + CursorYPosition);
      Console.WriteLine("WindowX\t" + windowXPosition);
      Console.WriteLine("WindowY\t" + windowYPosition);
      Console.WriteLine("CursorSize\t" + _cursorSize);
    }

    private bool IsCoordInCursor(int x, int y)
    {
      var xDiff = Math.Abs(x - CursorXPosition);
      var yDiff = Math.Abs(y - CursorYPosition);

      if (xDiff <= _cursorSize && yDiff <= _cursorSize)
      {
        return true;
      }

      return false;
    }

    public void HandleTileChangeKeyPressed(ConsoleKey keyPressed)
    {
      if (keyPressed == ConsoleKey.D1)
      {
        PaintSpecifiedTileType(CursorXPosition, CursorYPosition, EnumTileTypes.Snow);
      }
      else if (keyPressed == ConsoleKey.D2)
      {
        PaintSpecifiedTileType(CursorXPosition, CursorYPosition, EnumTileTypes.SnowWalked);
      }
      else if (keyPressed == ConsoleKey.D3)
      {
        PaintSpecifiedTileType(CursorXPosition, CursorYPosition, EnumTileTypes.Road);
      }
      else if (keyPressed == ConsoleKey.D4)
      {
        PaintSpecifiedTileType(CursorXPosition, CursorYPosition, EnumTileTypes.Grass);
      }
      else if (keyPressed == ConsoleKey.D5)
      {
        PaintSpecifiedTileType(CursorXPosition, CursorYPosition, EnumTileTypes.CabinFloor);
      }
      else if (keyPressed == ConsoleKey.D6)
      {
        PaintSpecifiedTileType(CursorXPosition, CursorYPosition, EnumTileTypes.CabinWall);
      }
      else if (keyPressed == ConsoleKey.D7)
      {
        PaintSpecifiedTileType(CursorXPosition, CursorYPosition, EnumTileTypes.CabinDoor);
      }
      else if (keyPressed == ConsoleKey.D8)
      {
        PaintSpecifiedTileType(CursorXPosition, CursorYPosition, EnumTileTypes.Tree);
      }
      else if (keyPressed == ConsoleKey.D9)
      {
        PaintSpecifiedTileType(CursorXPosition, CursorYPosition, EnumTileTypes.River);
      }
      else if (keyPressed == ConsoleKey.D0)
      {
        PaintSpecifiedTileType(CursorXPosition, CursorYPosition, EnumTileTypes.CabinWindow);
      }
      else if (keyPressed == ConsoleKey.Q)
      {
        PaintSpecifiedTileType(CursorXPosition, CursorYPosition, EnumTileTypes.Mountain);
      }
      else if (keyPressed == ConsoleKey.Z)
      {
        mapInfo.SpawnPoints.Add(new Spawn(EnumSpawnTypes.Player, CursorXPosition, CursorYPosition));
      }
    }

    private void PaintSpecifiedTileType(int xMid, int yMid, EnumTileTypes tileType)
    {
      for (int y = yMid - _cursorSize; y <= yMid + _cursorSize; y++)
      {
        for (int x = xMid - _cursorSize; x <= xMid + _cursorSize; x++)
        {
          mapInfo.TileSet[x, y].TileType = tileType;
        }
      }
    }
  }
}
