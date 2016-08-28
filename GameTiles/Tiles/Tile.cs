using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameTiles.Enums;
using SFML.Graphics;
using SFML.System;
using System.Xml.Serialization;

namespace GameTiles.Tiles
{
    [Serializable]
    public struct Tile
    {
        public EnumTileTypes TileType { get; set; }
        [NonSerialized]
        public Sprite TileSprite;
    }
}
