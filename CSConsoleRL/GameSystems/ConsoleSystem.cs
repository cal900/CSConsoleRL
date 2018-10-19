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
using SFML.Window;

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
            BroadcastMessage(new ConsoleReferenceEvent(commandList));
        }

        public override void AddEntity(Entity entity)
        {

        }

        public override void HandleMessage(IGameEvent gameEvent)
        {
            switch (gameEvent.EventName)
            {
                case "NextFrame":
                    NextFrame();
                    break;
                case "ToggleConsole":
                    consoleOn = !consoleOn;
                    break;
                case "KeyPressed":
                    HandleKeyPressed((Keyboard.Key)((KeyPressedEvent)gameEvent).EventParams[0]);
                    break;
            }
        }

        private void NextFrame()
        {
            
        }

        private void HandleKeyPressed(Keyboard.Key key)
        {
            if(key == Keyboard.Key.Return)
            {

            }
            else
            {

            }
        }

        private void CreateNewEvent(string commandText)
        {
            if (string.IsNullOrWhiteSpace(commandText)) return;

            string[] cmdParams = commandText.Split(' ');

            switch(cmdParams[0])
            {
                case "q":   //Quit Console
                    BroadcastMessage(new ToggleConsoleEvent());
                    break;
                case "togglefow":
                    BroadcastMessage(new ToggleFowEvent());
                    break;
            }
        }
    }
}