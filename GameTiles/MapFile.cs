using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameTiles.Tiles;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace GameTiles
{
    [Serializable()]
    public struct MapFile : ICloneable
    {
        public MapFile(Tile[,] _tileSet)
        { TileSet = _tileSet; }
        public Tile[,] TileSet;
    }
}
