using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameTiles.Tiles;

namespace CSConsoleRL.Game.Managers
{
  public class TileManager
  {
    public Tile[,] TileSet;

    GameSystemManager SystemManager;

    public TileManager(GameSystemManager manager)
    {
      SystemManager = manager;

      TileSet = new Tile[10, 10];
    }
  }
}
