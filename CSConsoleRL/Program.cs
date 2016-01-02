using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Graphics;
using CSConsoleRL.InputHandling;

namespace CSConsoleRL
{
    public class Program
    {
        private List<DrawableChar> DrawableChars;
        private DrawCharGraphics Graphics;
        private HandleUserInput UserInput;

        static void Main(string[] args)
        {
            Program MainProgram = new Program();

            MainProgram.InitializeClasses();
            MainProgram.GameLoop();
        }

        public void InitializeClasses()
        {
            DrawableChars = new List<DrawableChar>();
            Graphics = new DrawCharGraphics();
            UserInput = new HandleUserInput();

            //test
            DrawableChar player = new DrawableChar
            {
            Character = '@',
            XPositionOnMap = 0,
            YPositionOnMap = 0
            };
            DrawableChars.Add(player);

            DrawableChar fence = new DrawableChar
        {
            Character = '#',
            XPositionOnMap = 2,
            YPositionOnMap = 2
        };
            DrawableChars.Add(fence);

            DrawableChar road = new DrawableChar
            {
                Character = '=',
                XPositionOnMap = 6,
                YPositionOnMap = 6
            };
            DrawableChars.Add(road);

            DrawableChar sign = new DrawableChar
            {
                Character = 'S',
                XPositionOnMap = 7,
                YPositionOnMap = 7
            };
            DrawableChars.Add(sign);
            //
        }

        public void GameLoop()
        {
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {
                Graphics.DrawGraphics(0, 0, DrawableChars);
            }
        }
    }
}
