using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameTiles;
using GameTiles.Tiles;
using GameTiles.Enums;
using GameTiles.Spawns;
using Utilities;
using CSConsoleRL.Game.Managers;

namespace CSConsoleRL
{
  public class Program
  {
    static void Main(string[] args)
    {
      var mapInfo = new MapFile(new Tile[30, 30], new List<Spawn>());
      for (int y = 0; y < mapInfo.TileSet.GetLength(1); y++)
      {
        for (int x = 0; x < mapInfo.TileSet.GetLength(0); x++)
        {
          mapInfo.TileSet[x, y].TileType = EnumTileTypes.Snow;
        }
      }

      var mapIndex = args.Length > 0 ? args[0] : null;
      var newFileMenu = new MapFileHandler.MapFileHandler(ref mapInfo, mapIndex);

      //Set LOS default
      for (int y = 0; y < mapInfo.TileSet.GetLength(1); y++)
      {
        for (int x = 0; x < mapInfo.TileSet.GetLength(0); x++)
        {
          mapInfo.TileSet[x, y].IsInLos = false;
        }
      }

      var gameSystemManager = new GameSystemManager(mapInfo);
    }
  }
}