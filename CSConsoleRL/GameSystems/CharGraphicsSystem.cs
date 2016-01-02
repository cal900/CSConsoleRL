using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Game.Managers;
using CSConsoleRL.Components;
using CSConsoleRL.Components.Interfaces;

namespace CSConsoleRL.GameSystems
{
    public class CharGraphicsSystem : GameSystem
    {
        private const int xWindowCharWidth = 20;
        private const int yPlayableAreaCharHeight = 20;
        private const int xWindowPositionOnMap = 0;
        private const int yWindowPositionOnMap = 0;
        private const int yUiAreaCharHeight = 5;
        private const ConsoleColor foregroundColor = ConsoleColor.DarkGray;
        private const ConsoleColor backgroundColor = ConsoleColor.Black;

        private List<DrawableCharComponent> drawableComponents;

        public CharGraphicsSystem(GameSystemManager manager)
        {
            SystemManager = manager;

            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.Clear();

            Console.SetWindowSize(xWindowCharWidth, yPlayableAreaCharHeight + yUiAreaCharHeight);

            drawableComponents = new List<DrawableCharComponent>();
        }

        public void AddComponent(IComponent component)
        {
            drawableComponents.Add((component as DrawableCharComponent));
        }

        public void DrawCharGraphics(int XCurrentPositionOnMap, int YCurrentPositionOnMap)
        {
            Console.Clear();

            //Iterates through the array and draws character to console screen
            for (int index = 0; index < drawableComponents.Count; index++ )
            {
                Console.SetCursorPosition(drawableComponents[index].XPositionOnMap - XCurrentPositionOnMap, drawableComponents[index].YPositionOnMap - YCurrentPositionOnMap);
                Console.Write(drawableComponents[index].Character);
            }
        }
    }
}
