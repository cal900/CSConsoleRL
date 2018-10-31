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
        private class gameConsole
        {
            private int activeCommandIndex = -1;
            public List<string> Commands { get; private set; }
            public string ActiveCommand
            {
                get
                {
                    return Commands[Commands.Count - 1];
                }
                set
                {
                    Commands[Commands.Count - 1] = value;
                }
            }

            //Initialize with a string so ActiveCommand doesn't get messed up by Count of 0
            public gameConsole() { Commands = new List<string>() { ">" }; }

            public void NewLine()
            {
                Commands.Add("");
            }

            public void AddCommand()
            {
                Commands.Add(">");
                activeCommandIndex = -1;
            }

            public void WriteText(string text)
            {
                NewLine();
                ActiveCommand = string.Format("{0}", text);
            }

            //Not useful yet as currently what game writes to console is considered a "command"
            public void GoUpHistory()
            {
                if(activeCommandIndex < 0)
                {
                    ActiveCommand = Commands[Commands.Count - 2];
                }
                else if(activeCommandIndex > 0) //If active command is index 0, nowhere to go, only go up if higher than 0
                {
                    ActiveCommand = Commands[--activeCommandIndex];
                }
            }

            public void GoDownHistory()
            {
                if (activeCommandIndex < 0 || activeCommandIndex == Commands.Count - 1) //Active command is just what's being typed in, delete text
                {
                    ActiveCommand = ">";
                }
                else if (activeCommandIndex >= 0)
                {
                    ActiveCommand = Commands[++activeCommandIndex];
                }
            }
        }

        private bool consoleOn;
        private gameConsole console;

        public ConsoleSystem(GameSystemManager manager)
        {
            SystemManager = manager;
            console = new gameConsole();
        }

        public override void InitializeSystem()
        {
            BroadcastMessage(new ConsoleReferenceEvent(console.Commands));
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
                    if (consoleOn) HandleKeyPressed((Keyboard.Key)((KeyPressedEvent)gameEvent).EventParams[0]);
                    break;
                case "RequestConsoleData":
                    BroadcastMessage(new SendConsoleDataEvent(console.Commands));
                    break;
            }
        }

        private void NextFrame()
        {

        }

        private void HandleKeyPressed(Keyboard.Key key)
        {
            if (key == Keyboard.Key.Return)
            {
                if (console.ActiveCommand.Length > 1) CreateNewEvent(console.ActiveCommand.Substring(1)); //Start at index 1 to get rid of '>' char
                console.AddCommand();
            }
            else if (key == Keyboard.Key.Tilde)
            {
                BroadcastMessage(new ToggleConsoleEvent());
            }
            else if (key == Keyboard.Key.Escape)
            {
                BroadcastMessage(new ToggleConsoleEvent());
            }
            else if (key == Keyboard.Key.BackSpace)
            {
                if (console.ActiveCommand.Length > 1) console.ActiveCommand = console.ActiveCommand.Remove(console.ActiveCommand.Length - 1);
            }
            else if (key == Keyboard.Key.Up)
            {
                console.GoUpHistory();
            }
            else if (key == Keyboard.Key.Down)
            {
                console.GoDownHistory();
            }
            else if(key == Keyboard.Key.Space)
            {
                console.ActiveCommand += " ";
            }
            else
            {
                console.ActiveCommand += key.ToString().ToLower();
            }
        }

        private void CreateNewEvent(string commandText)
        {
            if (string.IsNullOrWhiteSpace(commandText)) return;

            string[] cmdParams = commandText.Split(' ');

            switch (cmdParams[0].ToLower())
            {
                case "q":   //Quit Console
                    BroadcastMessage(new ToggleConsoleEvent());
                    console.WriteText("Exiting console");
                    break;
                case "e":   //Exit game
                    BroadcastMessage(new ExitGameEvent());
                    console.WriteText("Exiting game");
                    break;
                case "togglefow":
                    BroadcastMessage(new ToggleFowEvent());
                    console.WriteText(string.Format("Toggling Fog of War (FOW)"));
                    break;
                default:
                    console.WriteText(string.Format("Unrecognized command: {0}", cmdParams[0]));
                    break;
            }
        }
    }
}