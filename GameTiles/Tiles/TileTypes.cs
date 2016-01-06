using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameTiles.Enums;

namespace GameTiles.Tiles
{
    public struct TileData
    {
        public TileData(char _char, ConsoleColor _color, bool _blockable)
        { Character = _char; Color = _color; Blockable = _blockable; }
        public readonly char Character;
        public readonly ConsoleColor Color;
        public readonly bool Blockable;
    }

    //Overriding Dictionary - automatically populated dictionary of tiletypes matched to the relevant struct
    public class TileTypeDictionary : Dictionary<EnumTileTypes, TileData>
    {
        public TileTypeDictionary() : base()
        {
            Add(EnumTileTypes.Snow, new TileData('~', ConsoleColor.Gray, false));
            Add(EnumTileTypes.SnowWalked, new TileData('8', ConsoleColor.Gray, false));
            Add(EnumTileTypes.Road, new TileData('-', ConsoleColor.DarkGray, false));
            Add(EnumTileTypes.Grass, new TileData(';', ConsoleColor.Green, false));
            Add(EnumTileTypes.CabinWall, new TileData('=', ConsoleColor.DarkMagenta, false));
            Add(EnumTileTypes.CabinFloor, new TileData('+', ConsoleColor.DarkMagenta, false));
        }
    }
}
