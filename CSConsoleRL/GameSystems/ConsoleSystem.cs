using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Game.Managers;
using CSConsoleRL.Components;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Events;
using CSConsoleRL.Entities;
using CSConsoleRL.Enums;

namespace CSConsoleRL.GameSystems
{
    public class ConsoleSystem : GameSystem
    {
        private bool consoleOn;
        private List<string> commandList;

        public ConsoleSystem(GameSystemManager manager)
        {
            SystemManager = manager;
            commandList = new List<string>();
        }

        public override void InitializeSystem()
        {

        }

        public override void AddEntity(Entity entity)
        {

        }

        public override void HandleMessage(IGameEvent gameEvent)
        {
            switch (gameEvent.EventName)
            {
                case "ToggleConsole":
                    consoleOn = !consoleOn;
                    break;
            }
        }

        public override void BroadcastMessage(IGameEvent evnt)
        {
            throw new NotImplementedException();
        }
    }
}