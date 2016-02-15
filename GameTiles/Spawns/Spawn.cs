using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameTiles.Enums;

namespace GameTiles.Spawns
{
    [Serializable()]
    public struct Spawn
    {
        public EnumSpawnTypes SpawnType;
        public int XPosition;
        public int YPosition;

        public Spawn(EnumSpawnTypes _spawnType, int _xPosition, int _yPosition)
        {
            SpawnType = _spawnType;
            XPosition = _xPosition;
            YPosition = _yPosition;
        }
    }
}
