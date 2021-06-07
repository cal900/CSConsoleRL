using CSConsoleRL.Components;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;
using CSConsoleRL.Game.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using CSConsoleRL.Helpers;

namespace CSConsoleRL.GameSystems
{
  public class HealthSystem : GameSystem
  {
    private readonly GameStateHelper _gameStateHelper;

    public HealthSystem(GameSystemManager manager, GameStateHelper gameStateHelper)
    {
      SystemManager = manager;
      _systemEntities = new List<Entity>();
      _gameStateHelper = gameStateHelper;
    }

    public override void InitializeSystem()
    {
      //_targetingPath = PlotCourse((new Vector2i(0, 0)), new Vector2i(15, 10));
    }

    public override void AddEntity(Entity entity)
    {

    }

    public override void HandleMessage(IGameEvent gameEvent)
    {
      switch (gameEvent.EventName)
      {
        case "EntityAttackCoords":
          EntityAttackCoords((EntityAttackCoordsEvent)gameEvent);
          break;
      }
    }

    private void EntityAttackCoords(EntityAttackCoordsEvent gameEvent)
    {

    }
  }
}