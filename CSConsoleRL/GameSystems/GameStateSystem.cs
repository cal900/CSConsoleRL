using CSConsoleRL.Components;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;
using CSConsoleRL.Enums;
using CSConsoleRL.Game.Managers;
using GameTiles.Tiles;
using SFML.Window;
using SFML.System;
using CSConsoleRL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSConsoleRL.GameSystems
{
  public class GameStateSystem : GameSystem
  {
    private GameStateHelper _gameStateHelper;
    public GameStateSystem(GameSystemManager manager, GameStateHelper gameStateHelper)
    {
      SystemManager = manager;
      _gameStateHelper = gameStateHelper;
      _systemEntities = new List<Entity>();
    }

    public override void InitializeSystem()
    {

    }

    public override void HandleMessage(IGameEvent gameEvent)
    {
      switch (gameEvent.EventName)
      {
        case "RequestChangeGameState":
          var newState = (EnumGameState)gameEvent.EventParams[0];
          ChangeState(newState);
          break;
        case "SnapCameraToEntity":
          var entity = (Entity)gameEvent.EventParams[0];
          _gameStateHelper.SetCameraCoords(
            entity.GetComponent<PositionComponent>().ComponentXPositionOnMap,
            entity.GetComponent<PositionComponent>().ComponentYPositionOnMap
          );
          break;
      }
    }

    public override void AddEntity(Entity entity)
    {

    }

    private void ChangeState(EnumGameState newState)
    {
      _gameStateHelper.SetGameState(newState);
      BroadcastMessage(new NotifyChangeGameStateEvent(newState));
    }
  }
}