using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameTiles.Tiles;
using GameTiles.Spawns;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace GameTiles
{
    [Serializable()]
    public struct MapFile : ICloneable
    {
        public Tile[,] TileSet;
        public List<Spawn> SpawnPoints;

        public MapFile(Tile[,] _tileSet, List<Spawn> _spawnPoints)
        { 
            TileSet = _tileSet;
            SpawnPoints = _spawnPoints;
        }

        public object Clone()
        {
            MapFile clonedMapFile = new MapFile(new Tile[TileSet.GetLength(0),TileSet.GetLength(1)], new List<Spawn>());

            for(int yIndex = 0; yIndex < TileSet.GetLength(1); yIndex++)
            {
                for (int xIndex = 0; xIndex < TileSet.GetLength(1); xIndex++)
                {
                    clonedMapFile.TileSet[xIndex,yIndex] = TileSet[xIndex,yIndex];
                }
            }

            for (int index = 0; index < SpawnPoints.Count; index++ )
            {
                clonedMapFile.SpawnPoints.Add(SpawnPoints[index]);
            }

            return clonedMapFile;
        }
    }
}
