using CSConsoleRL.Components;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;
using CSConsoleRL.Game.Managers;
using SFML.Window;
using System.Collections.Generic;

namespace CSConsoleRL.GameSystems
{
    public class TerminalSystem : GameSystem
    {
        private ShellSystem _shellSystem;

        private class gameTerminal
        {
            private int activeCommandIndex = -1;
            public List<string> TermLines { get; private set; }
            public string ActiveLine
            {
                get
                {
                    return TermLines[TermLines.Count - 1];
                }
                set
                {
                    TermLines[TermLines.Count - 1] = value;
                }
            }

            //Initialize with a string so ActiveCommand doesn't get messed up by Count of 0
            public gameTerminal() { TermLines = new List<string>() { ">" }; }

            public void NewLine()
            {
                TermLines.Add("");
            }

            public void AddLine(string line)
            {
                TermLines.Add(line);
            }

            public void AddCommand()
            {
                TermLines.Add(">");
            }

            public void WriteText(string text)
            {
                NewLine();
                ActiveLine = string.Format("{0}", text);
            }
        }

        private bool consoleOn;
        private gameTerminal console;

        public TerminalSystem(GameSystemManager manager)
        {
            SystemManager = manager;
            console = new gameTerminal();

            //Initiate ShellSystem (do it here as Terminal and Shell need each other, can't operate independently)
            _shellSystem = new ShellSystem(manager);
        }

        public override void InitializeSystem()
        {
            BroadcastMessage(new ConsoleReferenceEvent(console.TermLines));
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
                    BroadcastMessage(new SendConsoleDataEvent(console.TermLines));
                    break;
            }
        }

        private void NextFrame()
        {

        }

        private void ToggleConsole()
        {
            _shellSystem.Reset();
            BroadcastMessage(new ToggleConsoleEvent());
        }

        private void HandleKeyPressed(Keyboard.Key key)
        {
            if (key == Keyboard.Key.Return)
            {
                if (console.ActiveLine.Length > 1) console.AddLine(_shellSystem.HandleInput(console.ActiveLine.Substring(1))); //Start at index 1 to get rid of '>' char
                console.AddCommand();
            }
            else if (key == Keyboard.Key.Tilde)
            {
                ToggleConsole();
            }
            else if (key == Keyboard.Key.Escape)
            {
                ToggleConsole();
            }
            else if (key == Keyboard.Key.BackSpace)
            {
                if (console.ActiveLine.Length > 1) console.ActiveLine = console.ActiveLine.Remove(console.ActiveLine.Length - 1);
            }
            else if (key == Keyboard.Key.Up)
            {
                console.ActiveLine = ">" + _shellSystem.GetPrev();
            }
            else if (key == Keyboard.Key.Down)
            {
                console.ActiveLine = ">" + _shellSystem.GetNext();
            }
            else if (key == Keyboard.Key.Space)
            {
                console.ActiveLine += " ";
            }
            else if ((int)key >= 26 && (int)key <= 35)
            {
                console.ActiveLine += key.ToString().Substring(3);
            }
            else
            {
                console.ActiveLine += key.ToString().ToLower();
            }
        }
    }
}