using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Game.Managers;

namespace CSConsoleRL.GameSystems
{
    public class UserInputSystem : GameSystem
    {
        private const ConsoleKey InputUp = ConsoleKey.UpArrow;
        private const ConsoleKey InputDown = ConsoleKey.DownArrow;
        private const ConsoleKey InputLeft = ConsoleKey.LeftArrow;
        private const ConsoleKey InputRight = ConsoleKey.RightArrow;

        public UserInputSystem(GameSystemManager manager)
        {
            SystemManager = manager;
        }
    }
}
