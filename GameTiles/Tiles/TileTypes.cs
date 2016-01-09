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
        public TileData(char _char, ConsoleColor _color, bool _blocksMovement, bool _blocksVision)
        { Character = _char; Color = _color; BlocksMovement = _blocksMovement; BlocksVision = _blocksVision; }
        public readonly char Character;
        public readonly ConsoleColor Color;
        public readonly bool BlocksMovement;
        public readonly bool BlocksVision;
    }

    //Overriding Dictionary - automatically populated dictionary of tiletypes matched to the relevant struct
    public class TileTypeDictionary : Dictionary<EnumTileTypes, TileData>
    {
        public TileTypeDictionary() : base()
        {
            Add(EnumTileTypes.Snow, new TileData('~', ConsoleColor.Gray, false, false));
            Add(EnumTileTypes.SnowWalked, new TileData('8', ConsoleColor.Gray, false, false));
            Add(EnumTileTypes.Road, new TileData('-', ConsoleColor.DarkGray, false, false));
            Add(EnumTileTypes.Grass, new TileData(';', ConsoleColor.Green, false, false));
            Add(EnumTileTypes.CabinWall, new TileData('=', ConsoleColor.DarkMagenta, true, true));
            Add(EnumTileTypes.CabinFloor, new TileData('+', ConsoleColor.DarkMagenta, false, false));
            Add(EnumTileTypes.CabinDoor, new TileData('%', ConsoleColor.DarkMagenta, false, true));
            Add(EnumTileTypes.Tree, new TileData('T', ConsoleColor.DarkGreen, true, true));
            Add(EnumTileTypes.River, new TileData('^', ConsoleColor.Blue, true, false));
            Add(EnumTileTypes.CabinWindow, new TileData('_', ConsoleColor.DarkMagenta, true, false));
        }
    }
}
