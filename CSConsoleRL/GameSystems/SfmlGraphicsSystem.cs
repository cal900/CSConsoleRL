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
using GameTiles.Tiles;
using GameTiles.Enums;
using SFML.Window;
using SFML.Graphics;

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
        private Window sfmlWindow;
        private Tile[,] gameTiles;
        private int worldXLength;
        private int worldYLength;
        private int windowXPositionInWorld = 0;
        private int windowYPositionInWorld = 0;
        private const int windowXSize = 720;
        private const int windowYSize = 480;

        private Dictionary<EnumTileTypes, Texture> tileTextures;

        private List<Entity> graphicsEntities;

        public SfmlGraphicsSystem(GameSystemManager manager, Window _sfmlWindow, Tile[,] _gameTiles, int _worldXLength, int _worldYLength)
        {
            SystemManager = manager;

            sfmlWindow = _sfmlWindow;   //don't create window here as we need the events to be in user input system
            gameTiles = _gameTiles;
            worldXLength = _worldXLength;
            worldYLength = _worldYLength;
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
            throw new NotImplementedException();
        }

        public void DrawSfmlGraphics()
        {
            //Get all background tiles in view of current window position
            var startingTileXPosition = windowXPositionInWorld / tilePixelSize;
            var endingTileXPosition = startingTileXPosition + (windowXSize / tilePixelSize);
            var startingTileYPosition = windowYPositionInWorld / tilePixelSize;
            var endingTileYPosition = startingTileYPosition + (windowYSize / tilePixelSize);

            //Draw background tiles
            for(int x = startingTileXPosition; x < endingTileXPosition; x++)
            {
                for (int y = startingTileYPosition; y < endingTileYPosition; y++)
                {
                    gameTiles[x,y].TileType
                }
            }


        }

        private void LoadTextures()
        {
            LoadTileTextures();
        }

        private void LoadTileTextures()
        {
            string fileName = @"\Data\Sprites\Regular20x20.png";
            tileTextures.Add(EnumTileTypes.Grass, new Texture(fileName, new IntRect(0, 0, 20, 20)));
            tileTextures.Add(EnumTileTypes.Snow, new Texture(fileName, new IntRect(0, 0, 20, 20)));
            tileTextures.Add(EnumTileTypes.SnowWalked, new Texture(fileName, new IntRect(20, 20, 20, 20)));
            tileTextures.Add(EnumTileTypes.Road, new Texture(fileName, new IntRect(0, 0, 20, 20)));
            tileTextures.Add(EnumTileTypes.CabinWall, new Texture(fileName, new IntRect(0, 60, 20, 20)));
            tileTextures.Add(EnumTileTypes.CabinFloor, new Texture(fileName, new IntRect(0, 40, 20, 20)));
            tileTextures.Add(EnumTileTypes.Tree, new Texture(fileName, new IntRect(0, 20, 20, 20)));
            tileTextures.Add(EnumTileTypes.River, new Texture(fileName, new IntRect(0, 100, 20, 20)));
            tileTextures.Add(EnumTileTypes.CabinWindow, new Texture(fileName, new IntRect(0, 80, 20, 20)));
        }
    }
}
