using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Game.Managers;
using CSConsoleRL.Components;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;
using CSConsoleRL.Enums;
using GameTiles.Tiles;
using GameTiles.Enums;
using Utilities;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.IO;
using CSConsoleRL.Helpers;
using CSConsoleRL.Data;

namespace CSConsoleRL.GameSystems
{
  public class SfmlGraphicsSystem : GameSystem
  {
    private int _xWindowCharWidth = 20;
    private int _yPlayableAreaCharHeight = 20;
    private int _tilePixelSize = 20;
    private const int _xWindowPositionOnMap = 0;
    private const int _yWindowPositionOnMap = 0;
    private const int _yUiAreaCharHeight = 5;
    private const ConsoleColor foregroundColor = ConsoleColor.DarkGray;
    private const ConsoleColor backgroundColor = ConsoleColor.Black;
    private RenderWindow sfmlWindow;
    private List<string> consoleCommands;
    private Tile[,] gameTiles;
    private int worldXLength;
    private int worldYLength;
    private int windowXPositionInWorld = 0;
    private int windowYPositionInWorld = 0;
    private int _windowXSize = 620;
    private int _windowYSize = 620;
    private bool _showTerminal;
    private const int _terminalDisplayLines = 15;
    private List<string> _terminalLines;
    private const uint _termCharSize = 16;

    private TileTypeDictionary _tileDictionary;
    private SfmlTextureDictionary _textureDictionary;
    private CameraHelper _cameraHelper;

    private Font _gameFont;

    private Item _activeItem;

    public SfmlGraphicsSystem(GameSystemManager manager, RenderWindow _sfmlWindow, Tile[,] _gameTiles)
    {
      LoadGlobals();

      SystemManager = manager;

      sfmlWindow = _sfmlWindow;   //Don't create window here as we need the events to be in user input system
      gameTiles = _gameTiles;
      worldXLength = gameTiles.GetLength(0);
      worldYLength = gameTiles.GetLength(1);

      _systemEntities = new List<Entity>();
      var fontPath = @"G:\Programming\CSConsoleRL\Oct172018Try\CSConsoleRL\CSConsoleRL\bin\Debug\Data\Fonts\arial.ttf";
      if (!File.Exists(fontPath)) fontPath = @"D:\Programming\CSConsoleRL\Data\Fonts\arial.ttf";
      if (!File.Exists(fontPath)) fontPath = @"/home/jason/dev/CSConsoleRL/Data/Fonts/arial.ttf";
      _gameFont = new Font(fontPath);

      LoadTextures();
    }

    public override void InitializeSystem()
    {
      AssignTileSprites();
    }

    public override void AddEntity(Entity entity)
    {
      if (entity.Components.ContainsKey(typeof(PositionComponent))
          && (entity.Components.ContainsKey(typeof(DrawableSfmlComponent)) || entity.Components.ContainsKey(typeof(FadingSfmlComponent))))
      {
        _systemEntities.Add(entity);
        AssignSfmlGraphicsComponentSprite(entity);
      }
    }

    public override void HandleMessage(IGameEvent gameEvent)
    {
      switch (gameEvent.EventName)
      {
        case "NextFrame":
          NextFrame();
          break;
        case "ScreenPositionChange":
          ScreenPositionChange((int)gameEvent.EventParams[0], (int)gameEvent.EventParams[1]);
          break;
        case "SendTerminalData":
          _terminalLines = (List<string>)gameEvent.EventParams[0];
          break;
        case "ToggleConsole":
          _showTerminal = !_showTerminal;
          break;
        case "ConsoleReference":
          consoleCommands = (List<string>)gameEvent.EventParams[0];
          break;
        case "SnapCameraToEntity":
          var newEnt = (Entity)gameEvent.EventParams[0];
          if (_cameraHelper != null) _cameraHelper.SetEntity(newEnt);
          else _cameraHelper = new CameraHelper(newEnt);
          break;
        case "SendActiveItem":
          _activeItem = (Item)gameEvent.EventParams[0];
          break;
      }
    }

    private void NextFrame()
    {
      DrawSfmlGraphics();
      if (_showTerminal)
      {
        // Since drawing the lines relies on rendering, need to do the terminal line parsing here
        // Request lines want to see, and will trim down based on graphics in DrawConsole()
        BroadcastMessage(new RequestTerminalDataEvent(_terminalDisplayLines));
        DrawConsole();
      }

      sfmlWindow.Display();
    }

    private void ScreenPositionChange(int newX, int newY)
    {
      windowXPositionInWorld = newX;
      windowYPositionInWorld = newY;
    }

    private void DrawSfmlGraphics()
    {
      //If no _cameraHelper initialized have no reference point to draw scene
      if (_cameraHelper == null) return;

      //Find out pixel co-ordinates need to be drawn
      var xCoordOnMap = _cameraHelper.GetEntityXPositionOnMap();
      var yCoordOnMap = _cameraHelper.GetEntityYPositionOnMap();

      //Based on tile position, figure out pixel co-ords in world
      var xPixelOnMap = CoordsToPixels(xCoordOnMap);
      var yPixelOnMap = CoordsToPixels(yCoordOnMap);

      //Since camera should be centered, shift left and up
      xPixelOnMap = xPixelOnMap - (_windowXSize / 2);
      yPixelOnMap = yPixelOnMap - (_windowYSize / 2);

      //Slight shift to take into account tile size
      xPixelOnMap = xPixelOnMap + (_tilePixelSize / 2);
      yPixelOnMap = yPixelOnMap + (_tilePixelSize / 2);

      //xPixelOnMap = xPixelOnMap + xPixelOffset;
      //yPixelOnMap = yPixelOnMap + xPixelOffset;

      //Based on that, figure out what tile range that covers
      var startingTileXPosition = (int)Math.Floor(PixelsToCoords(xPixelOnMap));
      var startingTileYPosition = (int)Math.Floor(PixelsToCoords(yPixelOnMap));
      var endingTileXPosition = Math.Ceiling(PixelsToCoords(xPixelOnMap + _windowXSize));
      var endingTileYPosition = Math.Ceiling(PixelsToCoords(yPixelOnMap + _windowYSize));

      sfmlWindow.Clear(Color.Black);

      //Draw background tiles
      for (int x = startingTileXPosition; x < endingTileXPosition; x++)
      {
        if (x < 0 || x >= gameTiles.GetLength(0)) continue;
        for (int y = startingTileYPosition; y < endingTileYPosition; y++)
        {
          if (y < 0 || y >= gameTiles.GetLength(1)) continue;

          gameTiles[x, y].TileSprite.Position = new Vector2f(x * _tilePixelSize - xPixelOnMap, y * _tilePixelSize - yPixelOnMap);

          //Check if tile is in LOS
          if (gameTiles[x, y].IsInLos)
          {
            gameTiles[x, y].TileSprite.Color = new Color(255, 255, 255);
            sfmlWindow.Draw(gameTiles[x, y].TileSprite);
          }
          else
          {
            gameTiles[x, y].TileSprite.Color = new Color(100, 100, 100);
            sfmlWindow.Draw(gameTiles[x, y].TileSprite);
          }
        }
      }

      //Draw game sprites
      foreach (Entity entity in _systemEntities.Where(ent => ent.HasComponent<DrawableSfmlComponent>()))
      {
        var sfmlComponent = entity.GetComponent<DrawableSfmlComponent>();
        var positionComponent = entity.GetComponent<PositionComponent>();

        int spriteXPosition = (positionComponent.ComponentXPositionOnMap - windowXPositionInWorld) * _tilePixelSize;
        int spriteYPosition = (positionComponent.ComponentYPositionOnMap - windowYPositionInWorld) * _tilePixelSize;

        sfmlComponent.GameSprite.Position = new Vector2f(spriteXPosition - xPixelOnMap, spriteYPosition - yPixelOnMap);
        sfmlWindow.Draw(sfmlComponent.GameSprite);
      }

      //Draw animated sprites

      //Draw fading sprites
      foreach (Entity entity in _systemEntities.Where(ent => ent.HasComponent<FadingSfmlComponent>()))
      {
        var sfmlComponent = entity.GetComponent<FadingSfmlComponent>();
        var positionComponent = entity.GetComponent<PositionComponent>();

        if (sfmlComponent.ShouldDelete())
        {
          SystemManager.RemoveEntity(entity);
        }
        else
        {
          int spriteXPosition = (positionComponent.ComponentXPositionOnMap - windowXPositionInWorld) * _tilePixelSize;
          int spriteYPosition = (positionComponent.ComponentYPositionOnMap - windowYPositionInWorld) * _tilePixelSize;

          sfmlComponent.GameSprite.Position = new Vector2f(spriteXPosition - xPixelOnMap, spriteYPosition - yPixelOnMap);
          sfmlWindow.Draw(sfmlComponent.GameSprite);
          sfmlComponent.NextFrame();
        }
      }

      //Draw UI
      DrawUi();
    }

    private void DrawConsole()
    {
      //Console is black rect with white text
      var rect = new RectangleShape(new Vector2f(_windowXSize, _tilePixelSize * _terminalDisplayLines));
      rect.Position = new Vector2f(0, 0);
      rect.FillColor = new Color(0, 0, 0, 150);
      sfmlWindow.Draw(rect);

      //Draw console command text
      var textColor = new Color(255, 255, 255);
      //Iterate through commands, write line by line to console
      for (int i = _terminalLines.Count - 1; i >= 0; i--)
      {
        var lineStartYCoord = (_terminalDisplayLines - i) * _termCharSize;
        var lineText = new Text(_terminalLines[_terminalLines.Count - 1 - i], _gameFont, _termCharSize);
        lineText.Color = textColor;
        lineText.Position = new Vector2f(0, lineStartYCoord);
        sfmlWindow.Draw(lineText);
      }
    }

    private void DrawUi()
    {
      //Currently Active Item - Square in bottom right
      var borderRect = new RectangleShape(new Vector2f((float)(_tilePixelSize * 2 + 10), (float)(_tilePixelSize * 2 + 10)));
      var itemRect = new RectangleShape(new Vector2f((_tilePixelSize * 2), (_tilePixelSize * 2)));

      var itemXCoord = _windowXSize - (_tilePixelSize * 2) - 15;
      var itemYCoord = _windowYSize - (_tilePixelSize * 2) - 15;
      borderRect.Position = new Vector2f(itemXCoord, itemYCoord);
      itemRect.Position = new Vector2f(borderRect.Position.X + 5, borderRect.Position.Y + 5);

      borderRect.FillColor = new Color(155, 155, 0);
      itemRect.FillColor = new Color(25, 25, 25);

      //Get currently active item
      BroadcastMessage(new RequestActiveItemEvent(_cameraHelper.GetEntity().Id));
      var itemSprite = new Sprite(_textureDictionary[GetItemSpriteEnum(_activeItem.ItemType)]);
      itemSprite.Position = new Vector2f(borderRect.Position.X + 5, borderRect.Position.Y + 5);
      itemSprite.Scale = new Vector2f(2, 2);

      sfmlWindow.Draw(borderRect);
      sfmlWindow.Draw(itemRect);
      sfmlWindow.Draw(itemSprite);
    }

    private void LoadGlobals()
    {
      var xWindowCharWidth = GameGlobals.Instance().Get<long>("xWindowCharWidth");
      if (xWindowCharWidth > 0) _xWindowCharWidth = (int)xWindowCharWidth;

      var yPlayableAreaCharHeight = GameGlobals.Instance().Get<long>("yPlayableAreaCharHeight");
      if (yPlayableAreaCharHeight > 0) _yPlayableAreaCharHeight = (int)yPlayableAreaCharHeight;

      var tilePixelSize = GameGlobals.Instance().Get<long>("tilePixelSize");
      if (tilePixelSize > 0) _tilePixelSize = (int)tilePixelSize;

      var windowXSize = GameGlobals.Instance().Get<long>("windowXSize");
      if (windowXSize > 0) _windowXSize = (int)windowXSize;

      var windowYSize = GameGlobals.Instance().Get<long>("windowYSize");
      if (windowYSize > 0) _windowYSize = (int)windowYSize;
    }

    private void LoadTextures()
    {
      LoadTileTextures();
      LoadSfmlGraphicsComponentTextures();
    }

    private void LoadTileTextures()
    {
      _tileDictionary = new TileTypeDictionary();
    }

    private void LoadSfmlGraphicsComponentTextures()
    {
      _textureDictionary = new SfmlTextureDictionary();
    }

    private void AssignTileSprites()
    {
      //Draw background tiles
      for (int x = 0; x < gameTiles.GetLength(0); x++)
      {
        for (int y = 0; y < gameTiles.GetLength(1); y++)
        {
          gameTiles[x, y].TileSprite = new Sprite(_tileDictionary[gameTiles[x, y].TileType].Texture);
        }
      }
    }

    private void AssignSfmlGraphicsComponentSprite(Entity entity)
    {
      dynamic sfmlComponent = entity.GetComponent<DrawableSfmlComponent>();
      if (sfmlComponent == null)
      {
        sfmlComponent = entity.GetComponent<FadingSfmlComponent>();
      }
      switch (sfmlComponent.SpriteType)
      {
        case EnumSfmlSprites.MainCharacter:
          sfmlComponent.GameSprite = new Sprite(_textureDictionary[EnumSfmlSprites.MainCharacter]);
          break;
        case EnumSfmlSprites.HumanEnemy:
          sfmlComponent.GameSprite = new Sprite(_textureDictionary[EnumSfmlSprites.HumanEnemy]);
          break;
        case EnumSfmlSprites.Dog:
          sfmlComponent.GameSprite = new Sprite(_textureDictionary[EnumSfmlSprites.Dog]);
          break;
        case EnumSfmlSprites.RedX:
          sfmlComponent.GameSprite = new Sprite(_textureDictionary[EnumSfmlSprites.RedX]);
          break;
        case EnumSfmlSprites.GreenSquare:
          sfmlComponent.GameSprite = new Sprite(_textureDictionary[EnumSfmlSprites.GreenSquare]);
          break;
        case EnumSfmlSprites.YellowSquare:
          sfmlComponent.GameSprite = new Sprite(_textureDictionary[EnumSfmlSprites.YellowSquare]);
          break;
      }
    }

    private double PixelsToCoords(int pixels)
    {
      return pixels / _tilePixelSize;
    }

    private int CoordsToPixels(int coords)
    {
      return coords * _tilePixelSize;
    }

    private EnumSfmlSprites GetItemSpriteEnum(EnumItemTypes item)
    {
      switch (item)
      {
        case EnumItemTypes.GateKey:
          return EnumSfmlSprites.ItemKey;
        case EnumItemTypes.Knife:
          return EnumSfmlSprites.ItemKnife;
        case EnumItemTypes.Pistol:
          return EnumSfmlSprites.ItemPistol;
        case EnumItemTypes.SniperRifle:
          return EnumSfmlSprites.ItemPistol;
        default:
          return EnumSfmlSprites.ItemKnife;
      }
    }

    public class SfmlTextureDictionary : Dictionary<EnumSfmlSprites, Texture>
    {
      public SfmlTextureDictionary()
          : base()
      {
        string fileName = @"G:\Programming\CSConsoleRL\June302019Try\CSConsoleRL\Data\Sprites\Tiles20x20.png";
        if (!File.Exists(fileName)) fileName = @"D:\Programming\CSConsoleRL\Data\Sprites\Tiles20x20.png";
        if (!File.Exists(fileName)) fileName = @"/home/jason/dev/CSConsoleRL/Data/Sprites/Tiles20x20.png";
        Add(EnumSfmlSprites.MainCharacter, new Texture(fileName, new IntRect(0, 140, 20, 20)));
        Add(EnumSfmlSprites.HumanEnemy, new Texture(fileName, new IntRect(20, 140, 20, 20)));
        Add(EnumSfmlSprites.Dog, new Texture(fileName, new IntRect(0, 0, 20, 20)));
        Add(EnumSfmlSprites.RedX, new Texture(fileName, new IntRect(0, 160, 20, 20)));
        Add(EnumSfmlSprites.GreenSquare, new Texture(fileName, new IntRect(20, 160, 20, 20)));
        Add(EnumSfmlSprites.YellowSquare, new Texture(fileName, new IntRect(40, 160, 20, 20)));

        Add(EnumSfmlSprites.RoadLaneBot, new Texture(fileName, new IntRect(0, 380, 20, 20)));
        Add(EnumSfmlSprites.RoadLaneLeft, new Texture(fileName, new IntRect(20, 380, 20, 20)));
        Add(EnumSfmlSprites.RoadLaneTop, new Texture(fileName, new IntRect(40, 380, 20, 20)));
        Add(EnumSfmlSprites.RoadLaneRight, new Texture(fileName, new IntRect(60, 380, 20, 20)));

        Add(EnumSfmlSprites.Pavement, new Texture(fileName, new IntRect(0, 400, 20, 20)));
        Add(EnumSfmlSprites.MetalFence, new Texture(fileName, new IntRect(20, 400, 20, 20)));
        Add(EnumSfmlSprites.MetalFenceKeyhole, new Texture(fileName, new IntRect(40, 400, 20, 20)));

        Add(EnumSfmlSprites.CarTopLeft, new Texture(fileName, new IntRect(0, 420, 20, 20)));
        Add(EnumSfmlSprites.CarTopRight, new Texture(fileName, new IntRect(20, 420, 20, 20)));

        Add(EnumSfmlSprites.CarBotLeft, new Texture(fileName, new IntRect(0, 440, 20, 20)));
        Add(EnumSfmlSprites.CarBotRight, new Texture(fileName, new IntRect(20, 440, 20, 20)));

        Add(EnumSfmlSprites.ItemKnife, new Texture(fileName, new IntRect(0, 460, 20, 20)));
        Add(EnumSfmlSprites.ItemPistol, new Texture(fileName, new IntRect(20, 460, 20, 20)));
        Add(EnumSfmlSprites.ItemKey, new Texture(fileName, new IntRect(60, 460, 20, 20)));
      }
    }
  }
}
