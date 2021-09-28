using System.Collections.Generic;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;
using CSConsoleRL.Game.Managers;
using CSConsoleRL.Helpers;

namespace CSConsoleRL.GameSystems
{
  public class DebugSystem : GameSystem
  {
    private readonly GameStateHelper _gameStateHelper;

    public DebugSystem(GameSystemManager manager, GameStateHelper gameStateHelper)
    {
      SystemManager = manager;
      _systemEntities = new List<Entity>();
      _gameStateHelper = gameStateHelper;
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
        case "ToggleDebugMode":
          ToggleDebugMode((ToggleDebugModeEvent)gameEvent);
          break;
      }
    }

    private void ToggleDebugMode(ToggleDebugModeEvent gameEvent)
    {
      bool? debugMode = null;
      if (gameEvent.EventParams.Count > 0) debugMode = (bool?)gameEvent.EventParams[0];
      if (debugMode == null)
      {
        _gameStateHelper.SetDebugMode(!_gameStateHelper.DebugMode);
        return;
      }

      _gameStateHelper.SetDebugMode(debugMode.Value);
    }
  }
}