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
    public class UserInputSystem : GameSystem
    {
        private const ConsoleKey InputUp = ConsoleKey.UpArrow;
        private const ConsoleKey InputDown = ConsoleKey.DownArrow;
        private const ConsoleKey InputLeft = ConsoleKey.LeftArrow;
        private const ConsoleKey InputRight = ConsoleKey.RightArrow;

        private List<UserInputComponent> userControlComponents;

        public UserInputSystem(GameSystemManager manager)
        {
            SystemManager = manager;
        }

        public void AddComponent(IComponent component)
        {
            userControlComponents.Add((component as UserInputComponent));
        }
    }
}
