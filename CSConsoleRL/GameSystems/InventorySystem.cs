using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Game.Managers;
using CSConsoleRL.Components;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Events;
using CSConsoleRL.Entities;
using CSConsoleRL.Enums;
using GameTiles.Tiles;
using CSConsoleRL.Data;

namespace CSConsoleRL.GameSystems
{
  public class InventorySystem : GameSystem
  {
    public InventorySystem(GameSystemManager manager)
    {
      SystemManager = manager;
      _systemEntities = new List<Entity>();
    }

    public override void InitializeSystem()
    {

    }

    public override void AddEntity(Entity entity)
    {
      if (entity.Components.ContainsKey(typeof(InventoryComponent)))
      {
        _systemEntities.Add(entity);
      }
    }

    public override void HandleMessage(IGameEvent gameEvent)
    {
      switch (gameEvent.EventName)
      {
        case "RequestActiveItem":
          {
            var id = (Guid)gameEvent.EventParams[0];
            SystemManager.BroadcastEvent(new SendActiveItemEvent(GetActiveItemForEntity(id)));
            break;
          }
        case "ChangeActiveItem":
          {
            var id = (Guid)gameEvent.EventParams[0];
            var ent = _systemEntities.Where(e => e.Id == id).FirstOrDefault();
            if (ent != null) ent.GetComponent<InventoryComponent>().IncrementActiveItem();
            break;
          }
      }
    }

    private Item GetActiveItemForEntity(Guid id)
    {
      var entity = _systemEntities.Where(ent => ent.Id == id).First();
      var inventory = entity.GetComponent<InventoryComponent>();

      return inventory.GetActiveItem();
    }
  }
}
