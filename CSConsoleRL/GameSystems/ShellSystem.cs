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
      public Func<List<string>, List<string>> ShellFunction;
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

    public List<string> HandleInput(string input)
    {
      if (string.IsNullOrWhiteSpace(input)) return new List<string>() { "" };

      _shellHistory.AddInputToHistory(input);

      var inputArray = input.Split(' ');

      if (_supportedShellFunctions.ContainsKey(inputArray[0]))
      {
        try
        {
          var inputs = new List<string>(inputArray.Skip(1));

          return _supportedShellFunctions[inputArray[0]].ShellFunction(inputs);
        }
        catch (Exception e)
        {
          return new List<string>() { e.Message };
        }
      }
      else
      {
        return new List<string>() { string.Format("Command {0} not recognized, type 'help' for supported commands", input) };
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
        SupportedInputs = new List<string>() { "fow - fog of war, ai - ai for all npcs" },
        ShellFunction = Toggle
      });
      _supportedShellFunctions.Add("ce", new ShellCommand()
      {
        Desc = "Create Entity",
        SupportedInputs = new List<string>() { "marker, seeker, seekerPistol, seekerKnife, seekeraitest, fadingcolor", "x-coord", "y-coord" },
        ShellFunction = CreateEntity
      });
      _supportedShellFunctions.Add("le", new ShellCommand()
      {
        Desc = "List Entities",
        SupportedInputs = new List<string>() { "actor" },
        ShellFunction = ListEntities
      });
    }

    private List<string> ListShellCommands(List<string> inputs)
    {
      var verbose = (inputs != null && inputs.Count > 0 && inputs[0] == "v") ? true : false;
      var shellCommands = new List<string>();

      shellCommands.Add("");
      if (!verbose) shellCommands.Add("Add input 'v' for verbose help - shows all supported inputs for each command");

      foreach (var shellCmd in _supportedShellFunctions)
      {
        shellCommands.Add(shellCmd.Key + " - " + shellCmd.Value.Desc);

        if (verbose && shellCmd.Value.SupportedInputs != null && shellCmd.Value.SupportedInputs.Count > 0)
        {
          foreach (var input in shellCmd.Value.SupportedInputs)
          {
            shellCommands.Add('\t' + input);
          }
        }
      }

      shellCommands.Add("");

      return shellCommands;
    }

    private List<string> QuitTerminal(List<string> inputs)
    {
      BroadcastMessage(new ToggleConsoleEvent());
      return new List<string>() { "Exiting terminal" };
    }

    private List<string> ExitGame(List<string> inputs)
    {
      BroadcastMessage(new ExitGameEvent());
      return new List<string>() { "Exiting game" };
    }

    private List<string> Toggle(List<string> inputs)
    {
      var toggleArg = inputs[0];

      switch (toggleArg)
      {
        case "fow":
          BroadcastMessage(new ToggleFowEvent());
          return new List<string>() { string.Format("Toggling Fog of War (FOW)") };
        case "ai":
          BroadcastMessage(new ToggleAiEvent());
          return new List<string>() { string.Format("Toggling AI for NPCs") };
        default:
          return new List<string>() { string.Format("Unrecognized input to toggle: {0}", toggleArg) };
      }
    }

    private List<string> CreateEntity(List<string> inputs)
    {
      if (inputs.Count < 3)
      {
        return new List<string>() { string.Format("CreateEntity requires 3 inputs") };
      }

      switch (inputs[0])
      {
        case "marker":
          var markerEnt = new XMarkerEntity();
          markerEnt.GetComponent<PositionComponent>().ComponentXPositionOnMap = int.Parse(inputs[1]);
          markerEnt.GetComponent<PositionComponent>().ComponentYPositionOnMap = int.Parse(inputs[2]);
          SystemManager.RegisterEntity(markerEnt);
          return new List<string>() { string.Format("Creating entity {0} at co-ordinates [x->{1}, y->{2}]", inputs[0], inputs[1], inputs[2]) };
        case "seeker":
          var seekerEnt = new SeekerEntity();
          seekerEnt.GetComponent<PositionComponent>().ComponentXPositionOnMap = int.Parse(inputs[1]);
          seekerEnt.GetComponent<PositionComponent>().ComponentYPositionOnMap = int.Parse(inputs[2]);
          SystemManager.RegisterEntity(seekerEnt);
          return new List<string>() { string.Format("Creating entity {0} at co-ordinates [x->{1}, y->{2}]", inputs[0], inputs[1], inputs[2]) };
        case "seekerpistol":
          var seekerPistolEnt = new SeekerPistolEntity();
          seekerPistolEnt.GetComponent<PositionComponent>().ComponentXPositionOnMap = int.Parse(inputs[1]);
          seekerPistolEnt.GetComponent<PositionComponent>().ComponentYPositionOnMap = int.Parse(inputs[2]);
          SystemManager.RegisterEntity(seekerPistolEnt);
          return new List<string>() { string.Format("Creating entity {0} at co-ordinates [x->{1}, y->{2}]", inputs[0], inputs[1], inputs[2]) };
        case "seekerknife":
          var seekerKnifeEnt = new SeekerKnifeEntity();
          seekerKnifeEnt.GetComponent<PositionComponent>().ComponentXPositionOnMap = int.Parse(inputs[1]);
          seekerKnifeEnt.GetComponent<PositionComponent>().ComponentYPositionOnMap = int.Parse(inputs[2]);
          SystemManager.RegisterEntity(seekerKnifeEnt);
          return new List<string>() { string.Format("Creating entity {0} at co-ordinates [x->{1}, y->{2}]", inputs[0], inputs[1], inputs[2]) };
        case "seekeraitest":
          var seekerAiTestEnt = new SeekerAiTestEntity();
          seekerAiTestEnt.GetComponent<PositionComponent>().ComponentXPositionOnMap = int.Parse(inputs[1]);
          seekerAiTestEnt.GetComponent<PositionComponent>().ComponentYPositionOnMap = int.Parse(inputs[2]);
          SystemManager.RegisterEntity(seekerAiTestEnt);
          return new List<string>() { string.Format("Creating entity {0} at co-ordinates [x->{1}, y->{2}]", inputs[0], inputs[1], inputs[2]) };
        case "fadingcolor":
          var fadingColorEnt = new FadingColorEntity(inputs[1]);
          fadingColorEnt.GetComponent<PositionComponent>().ComponentXPositionOnMap = int.Parse(inputs[2]);
          fadingColorEnt.GetComponent<PositionComponent>().ComponentYPositionOnMap = int.Parse(inputs[3]);
          SystemManager.RegisterEntity(fadingColorEnt);
          return new List<string>() { string.Format("Creating entity {0} at co-ordinates [x->{1}, y->{2}]", inputs[0], inputs[2], inputs[3]) };
        default:
          return new List<string>() { string.Format("Specified entity {0} is unrecognized", inputs[0]) };
      }
    }

    private List<string> ListEntities(List<string> inputs)
    {
      var output = new List<string>();

      if (inputs.Count == 0)
      {
        var ents = SystemManager.GetEntities();
        foreach (var ent in ents)
        {
          var positionComponent = ((PositionComponent)ent.Components[typeof(PositionComponent)]);
          output.Add(string.Format("Id: {0}, x: {1}, y: {2}", ent.Id, positionComponent.ComponentXPositionOnMap, positionComponent.ComponentYPositionOnMap));
        }
      }
      else
      {
        switch (inputs[0])
        {
          case "actor":
            var ents = SystemManager.GetEntities<ActorEntity>();
            foreach (var ent in ents)
            {
              var positionComponent = ((PositionComponent)ent.Components[typeof(PositionComponent)]);
              output.Add(string.Format("Id: {0}, x: {1}, y: {2}", ent.Id, positionComponent.ComponentXPositionOnMap, positionComponent.ComponentYPositionOnMap));
            }
            break;
        }
      }

      return output;
    }
  }
}