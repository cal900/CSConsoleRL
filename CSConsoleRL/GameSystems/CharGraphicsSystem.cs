using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Game.Managers;
using CSConsoleRL.Components;

namespace CSConsoleRL.GameSystems
{
    public class CharGraphicsSystem : GameSystem
    {
        private const int XWindowCharWidth = 20;
        private const int YPlayableAreaCharHeight = 20;
        private const int XWindowPositionOnMap = 0;
        private const int YWindowPositionOnMap = 0;
        private const int YUiAreaCharHeight = 5;
        private const ConsoleColor ForegroundColor = ConsoleColor.DarkGray;
        private const ConsoleColor BackgroundColor = ConsoleColor.Black;

        public CharGraphicsSystem(GameSystemManager manager)
        {
            SystemManager = manager;

            Console.ForegroundColor = ForegroundColor;
            Console.BackgroundColor = BackgroundColor;
            Console.Clear();

            Console.SetWindowSize(XWindowCharWidth, YPlayableAreaCharHeight + YUiAreaCharHeight);
        }

        public void DrawCharGraphics(int XCurrentPositionOnMap, int YCurrentPositionOnMap)
        {
            Console.Clear();

            //Gets array of all characters that are in the viewable screen area
            var toDraw = SystemManager.DrawableCharComponents
            .Where(component => component.XPositionOnMap >= XCurrentPositionOnMap
            && component.XPositionOnMap < XCurrentPositionOnMap + XWindowCharWidth
            && component.YPositionOnMap >= YCurrentPositionOnMap
            && component.YPositionOnMap < YCurrentPositionOnMap + YPlayableAreaCharHeight)
            .ToArray();

            //Iterates through the array and draws character to console screen
            foreach (var currentComponent in toDraw)
            {
                Console.SetCursorPosition(currentComponent.XPositionOnMap - XCurrentPositionOnMap, currentComponent.YPositionOnMap - YCurrentPositionOnMap);
                Console.Write(currentComponent.Character);
            }
        }
    }
}
