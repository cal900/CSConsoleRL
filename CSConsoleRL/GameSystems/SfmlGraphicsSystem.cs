﻿using System;
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
    private GameStateHelper _gameStateHelper;
    private RenderWindow _sfmlWindow;
    private Tile[,] _gameTiles;
    private List<string> consoleCommands;
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

    public SfmlGraphicsSystem(GameSystemManager manager, GameStateHelper gameStateHelper, RenderWindow sfmlWindow, Tile[,] gameTiles)
    {
      LoadGlobals();

      SystemManager = manager;

      _gameStateHelper = gameStateHelper;
      _sfmlWindow = sfmlWindow;   //Don't create window here as we need the events to be in user input system
      _gameTiles = gameTiles;
      worldXLength = this._gameTiles.GetLength(0);
      worldYLength = this._gameTiles.GetLength(1);

      _systemEntities = new List<Entity>();
      var fontPath = @"G:\Programming\CSConsoleRL\Oct172018Try\CSConsoleRL\CSConsoleRL\bin\Debug\Data\Fonts\arial.ttf";
      if (!File.Exists(fontPath)) fontPath = @"H:\Programming\CSConsoleRL\Data\Fonts\arial.ttf";
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

      _sfmlWindow.Display();
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

      _sfmlWindow.Clear(Color.Black);

      //Draw background tiles
      for (int x = startingTileXPosition; x < endingTileXPosition; x++)
      {
        if (x < 0 || x >= _gameTiles.GetLength(0)) continue;
        for (int y = startingTileYPosition; y < endingTileYPosition; y++)
        {
          if (y < 0 || y >= _gameTiles.GetLength(1)) continue;

          _gameTiles[x, y].TileSprite.Position = new Vector2f(x * _tilePixelSize - xPixelOnMap, y * _tilePixelSize - yPixelOnMap);

          //Check if tile is in LOS
          if (_gameTiles[x, y].IsInLos)
          {
            _gameTiles[x, y].TileSprite.Color = new Color(255, 255, 255);
            _sfmlWindow.Draw(_gameTiles[x, y].TileSprite);
          }
          else
          {
            _gameTiles[x, y].TileSprite.Color = new Color(100, 100, 100);
            _sfmlWindow.Draw(_gameTiles[x, y].TileSprite);
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
        _sfmlWindow.Draw(sfmlComponent.GameSprite);
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
          _sfmlWindow.Draw(sfmlComponent.GameSprite);
          sfmlComponent.NextFrame();
        }
      }

      if (_gameStateHelper.GameState == EnumGameState.Targeting)
      {
        DrawTargetingPath(xPixelOnMap, yPixelOnMap);
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
      _sfmlWindow.Draw(rect);

      //Draw console command text
      var textColor = new Color(255, 255, 255);
      //Iterate through commands, write line by line to console
      for (int i = _terminalLines.Count - 1; i >= 0; i--)
      {
        var lineStartYCoord = (_terminalDisplayLines - i) * _termCharSize;
        var lineText = new Text(_terminalLines[_terminalLines.Count - 1 - i], _gameFont, _termCharSize);
        lineText.Color = textColor;
        lineText.Position = new Vector2f(0, lineStartYCoord);
        _sfmlWindow.Draw(lineText);
      }
    }

    private void DrawTargetingPath(int xPixelOnMap, int yPixelOnMap)
    {
      var targetingData = _gameStateHelper.GetVar<TargetingData>("TargetingData");
      var targetingPath = targetingData.Path;
      var target = targetingData.Target;

      if (targetingPath == null)
      {
        return;
      }

      var targetSprite = new Sprite(_textureDictionary[EnumSfmlSprites.TealX]);

      int spriteX = (target.X - windowXPositionInWorld) * _tilePixelSize;
      int spriteY = (target.Y - windowYPositionInWorld) * _tilePixelSize;
      targetSprite.Position = new Vector2f(spriteX - xPixelOnMap, spriteY - yPixelOnMap);
      _sfmlWindow.Draw(targetSprite);

      for (int i = 1; i < targetingPath.Count; i++)
      {
        var targetingSprite = new Sprite(_textureDictionary[EnumSfmlSprites.RedX]);

        int spriteXPosition = (targetingPath[i].X - windowXPositionInWorld) * _tilePixelSize;
        int spriteYPosition = (targetingPath[i].Y - windowYPositionInWorld) * _tilePixelSize;
        targetingSprite.Position = new Vector2f(spriteXPosition - xPixelOnMap, spriteYPosition - yPixelOnMap);

        _sfmlWindow.Draw(targetingSprite);
      }
    }

    private void DrawUi()
    {
      DrawHealthUi();
      DrawGameStateUi();
      DrawActiveItemUi();
    }

    private void DrawTextWithBorder(Text text, Color borderColor, Color backgroundColor, float borderStartingXPos, float borderThickness)
    {
      var boxWidth = text.GetLocalBounds().Width + (borderThickness * 4);
      var boxHeight = text.GetLocalBounds().Height + (borderThickness * 4);

      var borderVect = new Vector2f(boxWidth, boxHeight);
      var textVect = new Vector2f(boxWidth - (borderThickness * 2), boxHeight - (borderThickness * 2));

      var borderRect = new RectangleShape(borderVect);
      var textRect = new RectangleShape(textVect);
      borderRect.Position = new Vector2f(borderStartingXPos, _windowYSize - (_tilePixelSize * 2) - 15);
      textRect.Position = new Vector2f(borderStartingXPos + borderThickness, borderRect.Position.Y + borderThickness);
      borderRect.FillColor = borderColor;
      textRect.FillColor = backgroundColor;

      text.Position = new Vector2f(textRect.Position.X + borderThickness, textRect.Position.Y + (borderThickness / 2));

      _sfmlWindow.Draw(borderRect);
      _sfmlWindow.Draw(textRect);
      _sfmlWindow.Draw(text);
    }

    private void DrawHealthUi()
    {
      var mainEnt = _gameStateHelper.GetVar<Entity>("MainEntity");
      var healthComp = mainEnt.GetComponent<HealthComponent>();
      var maxHealth = healthComp.MaxHealth;
      var currentHealth = healthComp.CurrentHealth;

      var textStr = $"{currentHealth}/{maxHealth}";
      var text = new Text(textStr, _gameFont, _termCharSize);
      text.FillColor = new Color(255, 255, 255);

      DrawTextWithBorder(text, new Color(155, 155, 0), new Color(25, 25, 25), 15f, 5f);
    }

    private void DrawGameStateUi()
    {
      var borderThickness = 5;

      var textStr = "Not Set";
      switch (_gameStateHelper.GameState)
      {
        case EnumGameState.MainMenu:
          textStr = "Main Menu";
          break;
        case EnumGameState.RegularGame:
          textStr = "Regular Game";
          break;
        case EnumGameState.Targeting:
          textStr = "Targeting";
          break;
        default:
          break;
      }

      var text = new Text(textStr, _gameFont, _termCharSize);
      text.FillColor = new Color(255, 255, 255);

      var boxWidth = text.GetLocalBounds().Width + (borderThickness * 4);

      DrawTextWithBorder(text, new Color(155, 155, 0), new Color(25, 25, 25), (_windowXSize - boxWidth) / 2, 5f);
    }

    private void DrawActiveItemUi()
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

      _sfmlWindow.Draw(borderRect);
      _sfmlWindow.Draw(itemRect);
      _sfmlWindow.Draw(itemSprite);
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
      for (int x = 0; x < _gameTiles.GetLength(0); x++)
      {
        for (int y = 0; y < _gameTiles.GetLength(1); y++)
        {
          _gameTiles[x, y].TileSprite = new Sprite(_tileDictionary[_gameTiles[x, y].TileType].Texture);
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
        case EnumSfmlSprites.Seeker:
          sfmlComponent.GameSprite = new Sprite(_textureDictionary[EnumSfmlSprites.Seeker]);
          break;
        case EnumSfmlSprites.SeekerPistol:
          sfmlComponent.GameSprite = new Sprite(_textureDictionary[EnumSfmlSprites.SeekerPistol]);
          break;
        case EnumSfmlSprites.SeekerKnife:
          sfmlComponent.GameSprite = new Sprite(_textureDictionary[EnumSfmlSprites.SeekerKnife]);
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
        if (!File.Exists(fileName)) fileName = @"H:\Programming\CSConsoleRL\Data\Sprites\Tiles20x20.png";
        if (!File.Exists(fileName)) fileName = @"/home/jason/dev/CSConsoleRL/Data/Sprites/Tiles20x20.png";
        Add(EnumSfmlSprites.MainCharacter, new Texture(fileName, new IntRect(0, 140, 20, 20)));
        Add(EnumSfmlSprites.Seeker, new Texture(fileName, new IntRect(20, 140, 20, 20)));
        Add(EnumSfmlSprites.SeekerPistol, new Texture(fileName, new IntRect(40, 140, 20, 20)));
        Add(EnumSfmlSprites.SeekerKnife, new Texture(fileName, new IntRect(60, 140, 20, 20)));
        Add(EnumSfmlSprites.Dog, new Texture(fileName, new IntRect(0, 0, 20, 20)));

        Add(EnumSfmlSprites.RedX, new Texture(fileName, new IntRect(0, 160, 20, 20)));
        Add(EnumSfmlSprites.GreenSquare, new Texture(fileName, new IntRect(20, 160, 20, 20)));
        Add(EnumSfmlSprites.YellowSquare, new Texture(fileName, new IntRect(40, 160, 20, 20)));
        Add(EnumSfmlSprites.TealX, new Texture(fileName, new IntRect(60, 160, 20, 20)));

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
