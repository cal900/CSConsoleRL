using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameTiles.Enums;
using SFML.Graphics;
using SFML.System;

namespace GameTiles.Tiles
{
    [Serializable()]
    public struct Tile
    {
        public EnumTileTypes TileType { get; set; }
        public Sprite TileSprite { get; set; }
    }
}
