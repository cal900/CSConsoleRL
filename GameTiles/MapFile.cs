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

        public object Clone()
        {
            MapFile clonedMapFile = new MapFile(new Tile[TileSet.GetLength(0),TileSet.GetLength(1)]);

            for(int yIndex = 0; yIndex < TileSet.GetLength(1); yIndex++)
            {
                for (int xIndex = 0; xIndex < TileSet.GetLength(1); xIndex++)
                {
                    clonedMapFile.TileSet[xIndex,yIndex] = TileSet[xIndex,yIndex];
                }
            }

            return clonedMapFile;
        }
    }
}
