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
      if (entity.Components.ContainsKey(typeof(HealthComponent)))
      {
        _systemEntities.Add(entity);
      }
    }

    public override void HandleMessage(IGameEvent gameEvent)
    {
      switch (gameEvent.EventName)
      {
        case "ChangeEntityHealth":
          var guid = (Guid)gameEvent.EventParams[0];
          var ent = _systemEntities.Where(e => e.Id == guid).FirstOrDefault();
          var amount = (int)gameEvent.EventParams[1];
          ChangeEntityHealth(ent, amount);
          break;
      }
    }

    private void ChangeEntityHealth(Entity entity, int amount)
    {
      var healthComponent = entity.GetComponent<HealthComponent>();
      var currentHealth = healthComponent.CurrentHealth;
      var newHealth = currentHealth + amount;

      if (newHealth > healthComponent.MaxHealth)
      {
        newHealth = healthComponent.MaxHealth;
      }
      else if (newHealth < 0)
      {
        newHealth = 0;
      }

      // If health is 0, this entity is dead and needs to be removed
      if (newHealth == 0)
      {
        SystemManager.RemoveEntity(entity);
      }

      healthComponent.CurrentHealth = newHealth;
    }
  }
}