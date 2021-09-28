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
  public class CombatSystem : GameSystem
  {
    private readonly GameStateHelper _gameStateHelper;

    public CombatSystem(GameSystemManager manager, GameStateHelper gameStateHelper)
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
      if (entity.HasComponent<PositionComponent>() && entity.HasComponent<HealthComponent>())
      {
        _systemEntities.Add(entity);
      }
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
      int baseDamage = (int)gameEvent.EventParams[1];
      int targetX = (int)gameEvent.EventParams[2];
      int targetY = (int)gameEvent.EventParams[3];
      var entityAttacked = _systemEntities.Where(ent => ent.GetComponent<PositionComponent>().ComponentXPositionOnMap == targetX
        && ent.GetComponent<PositionComponent>().ComponentYPositionOnMap == targetY).FirstOrDefault();

      if (entityAttacked == null)
      {
        return;
      }
      Console.WriteLine($"Attack succeeded, changing HP by {baseDamage}");
      SystemManager.BroadcastEvent(new ChangeEntityHealthEvent(entityAttacked.Id, baseDamage * -1));
    }
  }
}