using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameTiles.Enums;
using SFML.Graphics;
using Utilities;

namespace GameTiles.Tiles
{
  public struct TileData
  {
    public TileData(char _char, Texture _texture, ConsoleColor _color, bool _blocksMovement, bool _blocksVision, bool blocksProjectiles)
    { Character = _char; Texture = _texture; Color = _color; BlocksMovement = _blocksMovement; BlocksVision = _blocksVision; BlocksProjectiles = blocksProjectiles; }
    public readonly char Character;
    public readonly Texture Texture;
    public readonly ConsoleColor Color;
    public readonly bool BlocksMovement;
    public readonly bool BlocksVision;
    public readonly bool BlocksProjectiles;
  }

  //Overriding Dictionary - automatically populated dictionary of tiletypes matched to the relevant struct
  public class TileTypeDictionary : Dictionary<EnumTileTypes, TileData>
  {
    public TileTypeDictionary() : base()
    {
      var fileName = GameGlobals.Instance().Get<string>("textureDir");
      if (!File.Exists(fileName)) fileName = @"G:\Programming\CSConsoleRL\CSConsoleRL\CSConsoleRL\bin\x64\Debug\Data\Sprites\Tiles20x20.png";
      if (!File.Exists(fileName)) fileName = @"F:\Programming\CSConsoleRL\Data\Sprites\Tiles20x20.png";
      if (!File.Exists(fileName)) fileName = @"/home/jason/dev/CSConsoleRL/Data/Sprites/Tiles20x20.png";
      Add(EnumTileTypes.Snow, new TileData('~', new Texture(fileName, new IntRect(0, 0, 20, 20)), ConsoleColor.Gray, false, false, false));
      Add(EnumTileTypes.SnowWalked, new TileData('8', new Texture(fileName, new IntRect(20, 20, 20, 20)), ConsoleColor.Gray, false, false, false));
      Add(EnumTileTypes.Road, new TileData('-', new Texture(fileName, new IntRect(0, 0, 20, 20)), ConsoleColor.DarkGray, false, false, false));
      Add(EnumTileTypes.Grass, new TileData(';', new Texture(fileName, new IntRect(0, 0, 20, 20)), ConsoleColor.Green, false, false, false));
      Add(EnumTileTypes.CabinWall, new TileData('=', new Texture(fileName, new IntRect(0, 60, 20, 20)), ConsoleColor.DarkMagenta, true, true, true));
      Add(EnumTileTypes.CabinFloor, new TileData('+', new Texture(fileName, new IntRect(0, 40, 20, 20)), ConsoleColor.DarkMagenta, false, false, false));
      Add(EnumTileTypes.CabinDoor, new TileData('%', new Texture(fileName, new IntRect(0, 40, 20, 20)), ConsoleColor.DarkMagenta, false, true, true));
      Add(EnumTileTypes.Tree, new TileData('T', new Texture(fileName, new IntRect(0, 20, 20, 20)), ConsoleColor.DarkGreen, true, true, true));
      Add(EnumTileTypes.River, new TileData('^', new Texture(fileName, new IntRect(0, 100, 20, 20)), ConsoleColor.Blue, true, false, false));
      Add(EnumTileTypes.CabinWindow, new TileData('_', new Texture(fileName, new IntRect(0, 80, 20, 20)), ConsoleColor.DarkMagenta, true, false, false));
      Add(EnumTileTypes.Mountain, new TileData('^', new Texture(fileName, new IntRect(40, 20, 20, 20)), ConsoleColor.DarkGray, true, true, true));
    }
  }
}
