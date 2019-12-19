using CSConsoleRL.Components;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;
using CSConsoleRL.Game.Managers;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSConsoleRL.GameSystems
{
    public class ShellSystem : GameSystem
    {
        struct ShellCommand
        {
            public string Desc;
            public List<string> SupportedInputs;
            public Func<List<string>, string> ShellFunction;
        }

        private class shellHistory
        {
            private int _historyIndex;
            private List<string> _history;

            public shellHistory()
            {
                _history = new List<string>();
            }

            public void AddInputToHistory(string input)
            {
                _history.Add(input);
                ResetHistory();
            }

            public string PrevHist()
            {
                if (_historyIndex > 0) _historyIndex--;
                return _history[_historyIndex];
            }

            public string NextHist()
            {
                if (_historyIndex < _history.Count - 1)
                {
                    _historyIndex++;
                    return _history[_historyIndex];
                }
                else
                {
                    _historyIndex = _history.Count;
                    return "";
                }
            }

            public void ResetHistory()
            {
                _historyIndex = _history.Count;
            }
        }

        private Dictionary<string, ShellCommand> _supportedShellFunctions;
        private shellHistory _shellHistory;

        public ShellSystem(GameSystemManager manager)
        {
            SystemManager = manager;
            DefineSupportedShellFunctions();
            _shellHistory = new shellHistory();
            _systemEntities = new List<Entity>();
        }

        public override void InitializeSystem()
        {

        }

        public override void AddEntity(Entity entity)
        {

        }

        public override void HandleMessage(IGameEvent gameEvent)
        {

        }

        public string HandleInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";

            _shellHistory.AddInputToHistory(input);

            var inputArray = input.Split(' ');

            if (_supportedShellFunctions.ContainsKey(inputArray[0]))
            {
                try
                {
                    var inputs = new List<string>(inputArray.Skip(1));

                    return _supportedShellFunctions[inputArray[0]].ShellFunction(inputs);
                }
                catch(Exception e)
                {
                    return e.Message;
                }
            }
            else
            {
                return string.Format("Command {0} not recognized, type 'help' for supported commands", input);
            }
        }

        public string GetPrev()
        {
            return _shellHistory.PrevHist();
        }

        public string GetNext()
        {
            return _shellHistory.NextHist();
        }

        public void Reset()
        {
            _shellHistory.ResetHistory();
        }

        /// <summary>
        /// Define supported shell functions here for use through the game terminal
        /// </summary>
        private void DefineSupportedShellFunctions()
        {
            _supportedShellFunctions = new Dictionary<string, ShellCommand>();

            var helpCmd = new ShellCommand()
            {
                Desc = "List Supported Commands",
                SupportedInputs = new List<string>() { "v - verbose, lists supported inputs for shell functions" },
                ShellFunction = ListShellCommands,
            };
            _supportedShellFunctions.Add("help", helpCmd);
            _supportedShellFunctions.Add("?", helpCmd);
            _supportedShellFunctions.Add("q", new ShellCommand()
            {
                Desc = "Quit Terminal",
                ShellFunction = QuitTerminal
            });
            _supportedShellFunctions.Add("e", new ShellCommand()
            {
                Desc = "Exit Game",
                ShellFunction = ExitGame
            });
            _supportedShellFunctions.Add("t", new ShellCommand()
            {
                Desc = "Toggle Specified System",
                SupportedInputs = new List<string>() { "fow - fog of war" },
                ShellFunction = Toggle
            });
            _supportedShellFunctions.Add("ce", new ShellCommand()
            {
                Desc = "Create Entity",
                SupportedInputs = new List<string>() { "marker, seeker, fadingcolor", "x-coord", "y-coord" },
                ShellFunction = CreateEntity
            });
            _supportedShellFunctions.Add("le", new ShellCommand()
            {
                Desc = "List Entities",
                SupportedInputs = new List<string>() { "actorentity" },
                ShellFunction = ListEntities
            });
        }

        private string ListShellCommands(List<string> inputs)
        {
            var verbose = (inputs != null && inputs.Count > 0 && inputs[0] == "v") ? true : false;
            var shellCommands = new StringBuilder();

            shellCommands.AppendLine("");
            if (!verbose) shellCommands.AppendLine("Add input 'v' for verbose help - shows all supported inputs for each command");

            foreach (var shellCmd in _supportedShellFunctions)
            {
                shellCommands.AppendLine(shellCmd.Key + " - " + shellCmd.Value.Desc);

                if (verbose && shellCmd.Value.SupportedInputs != null && shellCmd.Value.SupportedInputs.Count > 0)
                {
                    foreach (var input in shellCmd.Value.SupportedInputs)
                    {
                        shellCommands.AppendLine('\t' + input);
                    }
                }
            }

            return shellCommands.ToString();
        }

        private string QuitTerminal(List<string> inputs)
        {
            BroadcastMessage(new ToggleConsoleEvent());
            return "Exiting terminal";
        }

        private string ExitGame(List<string> inputs)
        {
            BroadcastMessage(new ExitGameEvent());
            return "Exiting game";
        }

        private string Toggle(List<string> inputs)
        {
            var toggleArg = inputs[0];

            switch (toggleArg)
            {
                case "fow":
                    BroadcastMessage(new ToggleFowEvent());
                    return string.Format("Toggling Fog of War (FOW)");
                default:
                    return string.Format("Unrecognized input to toggle: {0}", toggleArg);
            }
        }

        private string CreateEntity(List<string> inputs)
        {
            if (inputs.Count < 3)
            {
                return string.Format("CreateEntity requires 3 inputs");
            }

            switch (inputs[0])
            {
                case "marker":
                    var markerEnt = new XMarkerEntity();
                    markerEnt.GetComponent<PositionComponent>().ComponentXPositionOnMap = int.Parse(inputs[1]);
                    markerEnt.GetComponent<PositionComponent>().ComponentYPositionOnMap = int.Parse(inputs[2]);
                    SystemManager.RegisterEntity(markerEnt);
                    return string.Format("Creating entity {0} at co-ordinates [x->{1}, y->{2}]", inputs[0], inputs[1], inputs[2]);
                case "seeker":
                    var seekerEnt = new SeekerEntity();
                    seekerEnt.GetComponent<PositionComponent>().ComponentXPositionOnMap = int.Parse(inputs[1]);
                    seekerEnt.GetComponent<PositionComponent>().ComponentYPositionOnMap = int.Parse(inputs[2]);
                    SystemManager.RegisterEntity(seekerEnt);
                    return string.Format("Creating entity {0} at co-ordinates [x->{1}, y->{2}]", inputs[0], inputs[1], inputs[2]);
                case "fadingcolor":
                    var fadingColorEnt = new FadingColorEntity(inputs[1]);
                    fadingColorEnt.GetComponent<PositionComponent>().ComponentXPositionOnMap = int.Parse(inputs[2]);
                    fadingColorEnt.GetComponent<PositionComponent>().ComponentYPositionOnMap = int.Parse(inputs[3]);
                    SystemManager.RegisterEntity(fadingColorEnt);
                    return string.Format("Creating entity {0} at co-ordinates [x->{1}, y->{2}]", inputs[0], inputs[2], inputs[3]);
                default:
                    return string.Format("Specified entity {0} is unrecognized", inputs[0]);
            }
        }

        private string ListEntities(List<string> inputs)
        {
            if (inputs.Count < 1)
            {
                return string.Format("ListEntities requires 1 input");
            }

            var output = new StringBuilder();

            switch (inputs[0])
            {
                case "actorentity":
                    var ents = SystemManager.GetEntities<ActorEntity>();
                    foreach (var ent in ents)
                    {
                        var positionComponent = ((PositionComponent)ent.Components[typeof(PositionComponent)]);
                        output.AppendLine(string.Format("Id: {0}, x: {1}, y: {2}", ent.Id, positionComponent.ComponentXPositionOnMap, positionComponent.ComponentYPositionOnMap));
                    }
                    break;
            }

            return output.ToString();
        }
    }
}