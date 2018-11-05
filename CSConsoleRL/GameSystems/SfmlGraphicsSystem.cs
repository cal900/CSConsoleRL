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
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.IO;

namespace CSConsoleRL.GameSystems
{
    public class SfmlGraphicsSystem : GameSystem
    {
        private const int xWindowCharWidth = 20;
        private const int yPlayableAreaCharHeight = 20;
        private const int tilePixelSize = 20;
        private const int xWindowPositionOnMap = 0;
        private const int yWindowPositionOnMap = 0;
        private const int yUiAreaCharHeight = 5;
        private const ConsoleColor foregroundColor = ConsoleColor.DarkGray;
        private const ConsoleColor backgroundColor = ConsoleColor.Black;
        private RenderWindow sfmlWindow;
        private List<string> consoleCommands;
        private Tile[,] gameTiles;
        private int worldXLength;
        private int worldYLength;
        private int windowXPositionInWorld = 0;
        private int windowYPositionInWorld = 0;
        private const int windowXSize = 600;
        private const int windowYSize = 600;
        private bool showConsole;
        private const int consoleDisplayLines = 5;
        private List<string> console;

        private TileTypeDictionary _tileDictionary;
        private SfmlTextureDictionary _textureDictionary;

        private Font _gameFont;

        private List<Entity> _graphicsEntities;

        public SfmlGraphicsSystem(GameSystemManager manager, RenderWindow _sfmlWindow, Tile[,] _gameTiles)
        {
            SystemManager = manager;

            sfmlWindow = _sfmlWindow;   //Don't create window here as we need the events to be in user input system
            gameTiles = _gameTiles;
            worldXLength = gameTiles.GetLength(0);
            worldYLength = gameTiles.GetLength(1);

            _graphicsEntities = new List<Entity>();
            var fontPath = @"G:\Programming\CSConsoleRL\Oct172018Try\CSConsoleRL\CSConsoleRL\bin\Debug\Data\Fonts\arial.ttf";
            if (!File.Exists(fontPath)) fontPath = @"D:\Programming\CSConsoleRL\Data\Fonts\arial.ttf";
            _gameFont = new Font(fontPath);
        }

        public override void InitializeSystem()
        {
            LoadTextures();
            AssignSprites();
        }

        public override void AddEntity(Entity entity)
        {
            if (entity.Components.ContainsKey(typeof(PositionComponent)) && entity.Components.ContainsKey(typeof(DrawableSfmlComponent)))
            {
                _graphicsEntities.Add(entity);
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
                case "SendConsoleData":
                    console = (List<string>)gameEvent.EventParams[0];
                    break;
                case "ToggleConsole":
                    showConsole = !showConsole;
                    break;
                case "ConsoleReference":
                    consoleCommands = (List<string>)gameEvent.EventParams[0];
                    break;
            }
        }

        private void NextFrame()
        {
            DrawSfmlGraphics();
            if (showConsole)
            {
                BroadcastMessage(new RequestConsoleDataEvent(consoleDisplayLines));
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
            //Get all background tiles in view of current window position
            var startingTileXPosition = windowXPositionInWorld / tilePixelSize;
            var endingTileXPosition = startingTileXPosition + (windowXSize / tilePixelSize);
            var startingTileYPosition = windowYPositionInWorld / tilePixelSize;
            var endingTileYPosition = startingTileYPosition + (windowYSize / tilePixelSize);

            sfmlWindow.Clear(Color.Black);

            //Draw background tiles
            for (int x = startingTileXPosition; x < endingTileXPosition; x++)
            {
                for (int y = startingTileYPosition; y < endingTileYPosition; y++)
                {
                    gameTiles[x, y].TileSprite.Position = new Vector2f(x * tilePixelSize, y * tilePixelSize);

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
            foreach (Entity entity in _graphicsEntities)
            {
                var sfmlComponent = entity.GetComponent<DrawableSfmlComponent>();
                var positionComponent = entity.GetComponent<PositionComponent>();

                int spriteXPosition = (positionComponent.ComponentXPositionOnMap - windowXPositionInWorld) * tilePixelSize;
                int spriteYPosition = (positionComponent.ComponentYPositionOnMap - windowYPositionInWorld) * tilePixelSize;

                sfmlComponent.GameSprite.Position = new Vector2f(spriteXPosition, spriteYPosition);
                sfmlWindow.Draw(sfmlComponent.GameSprite);
            }
        }

        private void DrawTileEffects()
        {

        }

        private void DrawConsole()
        {
            //Console is black rect with white text
            var rect = new RectangleShape(new Vector2f(windowXSize, tilePixelSize * consoleDisplayLines));
            rect.Position = new Vector2f(0, 0);
            rect.FillColor = new Color(0, 0, 0, 150);
            sfmlWindow.Draw(rect);

            //Draw console command text
            var textColor = new Color(255, 255, 255);
            //Iterate through commands, write line by line to console
            for (int i = console.Count - 1; i >= 0; i--)
            {
                var lineStartYCoord = (consoleDisplayLines - i - 1) * yPlayableAreaCharHeight;
                var lineText = new Text(console[console.Count - 1 - i], _gameFont, 16);
                lineText.Color = textColor;
                lineText.Position = new Vector2f(0, lineStartYCoord);
                sfmlWindow.Draw(lineText);
            }
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

        private void AssignSprites()
        {
            AssignTileSprites();
            AssignSfmlGraphicsComponentSprites();
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

        private void AssignSfmlGraphicsComponentSprites()
        {
            foreach (Entity entity in _graphicsEntities)
            {
                var sfmlComponent = entity.GetComponent<DrawableSfmlComponent>();
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
                }
            }
        }

        public class SfmlTextureDictionary : Dictionary<EnumSfmlSprites, Texture>
        {
            public SfmlTextureDictionary()
                : base()
            {
                string fileName = @"G:\Programming\GitRepos\CSConsoleRL\CSConsoleRL\bin\x64\Debug\Data\Sprites\Tiles20x20.png";
                if (!File.Exists(fileName)) fileName = @"D:\Programming\CSConsoleRL\Data\Sprites\Tiles20x20.png";
                Add(EnumSfmlSprites.MainCharacter, new Texture(fileName, new IntRect(0, 140, 20, 20)));
                Add(EnumSfmlSprites.HumanEnemy, new Texture(fileName, new IntRect(0, 0, 20, 20)));
                Add(EnumSfmlSprites.Dog, new Texture(fileName, new IntRect(0, 0, 20, 20)));
                Add(EnumSfmlSprites.RedX, new Texture(fileName, new IntRect(0, 160, 20, 20)));
            }
        }
    }
}
