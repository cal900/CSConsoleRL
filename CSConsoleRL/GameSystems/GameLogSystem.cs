using CSConsoleRL.Components;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;
using CSConsoleRL.Game.Managers;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace CSConsoleRL.GameSystems
{
  public class GameLogSystem : GameSystem
  {
    private bool _gameLogEnabled;
    private List<string> _messages;
    private int _activeMsgIndex;
    private int _visibleMessages;

    public GameLogSystem(GameSystemManager manager)
    {
      SystemManager = manager;
      _systemEntities = new List<Entity>();
      _messages = new List<string>()
      {
        "Game Started"
      };
      _activeMsgIndex = 0;
      AddGameLogMessage("<b>Bold</b>");
      AddGameLogMessage("<C=#f10203ff>Red <b>Bold Red </b>Red</C>");
      _visibleMessages = 4;
    }

    private void AddGameLogMessage(string msg)
    {
      _messages.Add(msg);
      _activeMsgIndex = _messages.Count - 1;
    }

    public List<string> RequestGameLogMessages()
    {
      if (_activeMsgIndex < _visibleMessages)
      {
        return _messages.GetRange(0, _activeMsgIndex + 1);
      }

      return _messages.GetRange(_activeMsgIndex - _visibleMessages, _visibleMessages);
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
        case "AddGameLogMessage":
          var msg = (string)gameEvent.EventParams[0];
          AddGameLogMessage(msg);
          break;
        case "RequestGameLogMessages":
          var msgs = RequestGameLogMessages();
          BroadcastMessage(new SendGameLogMessagesEvent(msgs));
          break;
        case "ToggleGameLog":
          _gameLogEnabled = !_gameLogEnabled;
          break;
      }
    }

    private void NextFrame()
    {

    }
  }
}