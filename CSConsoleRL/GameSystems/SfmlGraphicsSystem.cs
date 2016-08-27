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
        private Tile[,] gameTiles;
        private int worldXLength;
        private int worldYLength;
        private int windowXPositionInWorld = 0;
        private int windowYPositionInWorld = 0;
        private const int windowXSize = 720;
        private const int windowYSize = 480;

        private TileTypeDictionary tileDictionary;
        private SfmlTextureDictionary textureDictionary;

        private List<Entity> graphicsEntities;

        public SfmlGraphicsSystem(GameSystemManager manager, RenderWindow _sfmlWindow, Tile[,] _gameTiles, int _worldXLength, int _worldYLength)
        {
            SystemManager = manager;

            sfmlWindow = _sfmlWindow;   //don't create window here as we need the events to be in user input system
            gameTiles = _gameTiles;
            worldXLength = _worldXLength;
            worldYLength = _worldYLength;

            LoadTextures();
        }

        public override void InitializeSystem()
        {
            LoadTextures();
            AssignSprites();
        }

        public override void AddEntity(Entity entity)
        {
            if(entity.Components.ContainsKey(typeof(PositionComponent)) && entity.Components.ContainsKey(typeof(DrawableSfmlComponent)))
            {
                graphicsEntities.Add(entity);
            }
        }

        public override void HandleMessage(GameEvent gameEvent)
        {
            switch(gameEvent.EventName)
            {
                case "NextFrame":
                    NextFrame();
                    break;
                case "ScreenPositionChange":
                    ScreenPositionChange((int)gameEvent.EventParams[0], (int)gameEvent.EventParams[1]);
                    break;
            }
        }

        public override GameEvent BroadcastMessage(GameEvent evnt)
        {
            throw new NotImplementedException();
        }

        private void NextFrame()
        {
            DrawSfmlGraphics();
        }

        private void ScreenPositionChange(int newX, int newY)
        {
            windowXPositionInWorld = newX;
            windowYPositionInWorld = newY;
        }

        public void DrawSfmlGraphics()
        {
            //Get all background tiles in view of current window position
            var startingTileXPosition = windowXPositionInWorld / tilePixelSize;
            var endingTileXPosition = startingTileXPosition + (windowXSize / tilePixelSize);
            var startingTileYPosition = windowYPositionInWorld / tilePixelSize;
            var endingTileYPosition = startingTileYPosition + (windowYSize / tilePixelSize);

            sfmlWindow.Clear();

            //Draw background tiles
            for(int x = startingTileXPosition; x < endingTileXPosition; x++)
            {
                for (int y = startingTileYPosition; y < endingTileYPosition; y++)
                {
                    gameTiles[x, y].TileSprite.Position = new Vector2f(x * tilePixelSize, y * tilePixelSize);
                    sfmlWindow.Draw(gameTiles[x, y].TileSprite);
                }
            }

            //draw game sprites
            foreach(Entity entity in graphicsEntities)
            {
                var sfmlComponent = entity.GetComponent<DrawableSfmlComponent>();
                var positionComponent = entity.GetComponent<PositionComponent>();

                int spriteXPosition = positionComponent.ComponentXPositionOnMap - windowXPositionInWorld;
                int spriteYPosition = positionComponent.ComponentYPositionOnMap - windowYPositionInWorld;

                sfmlComponent.GameSprite.Position = new Vector2f(spriteXPosition, spriteYPosition);
                sfmlWindow.Draw(sfmlComponent.GameSprite);
            }
        }

        private void LoadTextures()
        {
            LoadTileTextures();
            LoadSfmlGraphicsComponentTextures();
        }

        private void LoadTileTextures()
        {
            tileDictionary = new TileTypeDictionary();
        }

        private void LoadSfmlGraphicsComponentTextures()
        {
            textureDictionary = new SfmlTextureDictionary();
        }

        private void AssignSprites()
        {
            AssignTileSprites();
            AssignSfmlGraphicsComponentSprites();
        }

        private void AssignTileSprites()
        {
            //Draw background tiles
            for(int x = 0; x < gameTiles.GetLength(0); x++)
            {
                for (int y = 0; y < gameTiles.GetLength(1); y++)
                {
                    gameTiles[x, y].TileSprite = new Sprite(tileDictionary[gameTiles[x, y].TileType].Texture);
                }
            }
        }

        private void AssignSfmlGraphicsComponentSprites()
        {
            foreach(Entity entity in graphicsEntities)
            {
                var sfmlComponent = entity.GetComponent<DrawableSfmlComponent>();
                switch (sfmlComponent.SpriteType)
                {
                    case EnumSfmlSprites.MainCharacter:
                        sfmlComponent.GameSprite = new Sprite(textureDictionary[EnumSfmlSprites.MainCharacter]);
                        break;
                    case EnumSfmlSprites.HumanEnemy:
                        sfmlComponent.GameSprite = new Sprite(textureDictionary[EnumSfmlSprites.HumanEnemy]);
                        break;
                    case EnumSfmlSprites.Dog:
                        sfmlComponent.GameSprite = new Sprite(textureDictionary[EnumSfmlSprites.Dog]);
                        break;
                }
            }
        }

        public class SfmlTextureDictionary : Dictionary<EnumSfmlSprites, Texture>
        {
            public SfmlTextureDictionary()
                : base()
            {
                string fileName = @"\Data\Sprites\Regular20x20.png";
                Add(EnumSfmlSprites.MainCharacter, new Texture(fileName, new IntRect(0, 0, 20, 20)));
                Add(EnumSfmlSprites.HumanEnemy, new Texture(fileName, new IntRect(0, 0, 20, 20)));
                Add(EnumSfmlSprites.Dog, new Texture(fileName, new IntRect(0, 0, 20, 20)));
            }
        }
    }
}
