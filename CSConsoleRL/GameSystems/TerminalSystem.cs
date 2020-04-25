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
      private int _termViewportStart;
      private List<string> _termLines;
      public string ActiveLine
      {
        get
        {
          return _termLines[_termLines.Count - 1];
        }
        set
        {
          _termLines[_termLines.Count - 1] = value;
          _termViewportStart = _termLines.Count;
        }
      }

      // Initialize with a string so ActiveCommand doesn't get messed up by Count of 0
      public gameTerminal() { _termLines = new List<string>() { ">" }; }

      public List<string> GetTermLines(int numLines)
      {
        var requestedTermLines = new List<string>();

        // If we have less terminal lines than what will fill up terminal area, return lines from top downwards
        if (numLines >= _termLines.Count)
        {
          requestedTermLines = _termLines;
        }
        else
        {
          // If viewport is so low would go past terminal lines, just return bottom terminal lines equal to numLines
          if (_termViewportStart + numLines >= _termLines.Count)
          {
            requestedTermLines = _termLines.GetRange(_termLines.Count - numLines, numLines);
          }
          else
          {
            requestedTermLines = _termLines.GetRange(_termViewportStart, numLines);
          }
        }

        return requestedTermLines;
      }

      public void NewLine()
      {
        _termLines.Add("");
      }

      public void AddLine(string line)
      {
        _termLines.Add(line);
      }

      public void AddCommand()
      {
        _termLines.Add(">");
      }

      public void WriteText(string text)
      {
        NewLine();
        ActiveLine = string.Format("{0}", text);
      }

      public void ViewportUp()
      {
        if (_termViewportStart > 0) _termViewportStart--;
      }

      public void ViewportDown()
      {
        if (_termViewportStart < _termLines.Count - 1) _termViewportStart++;
      }
    }

    private bool consoleOn;
    private gameTerminal console;

    public TerminalSystem(GameSystemManager manager)
    {
      SystemManager = manager;
      console = new gameTerminal();

      _systemEntities = new List<Entity>();

      // Initiate ShellSystem (do it here as Terminal and Shell need each other, can't operate independently)
      _shellSystem = new ShellSystem(manager);
    }

    public override void InitializeSystem()
    {
      // BroadcastMessage(new TerminalReferenceEvent(console.TermLines));
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
        case "RequestTerminalData":
          BroadcastMessage(new SendTerminalDataEvent(console.GetTermLines((int)gameEvent.EventParams[0])));
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
      if (key == Keyboard.Key.Enter)
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
      else if (key == Keyboard.Key.Backspace)
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
      else if (key == Keyboard.Key.PageUp)
      {
        console.ViewportUp();
      }
      else if (key == Keyboard.Key.PageDown)
      {
        console.ViewportDown();
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