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

namespace CSConsoleRL.GameSystems
{
  public class InventorySystem : GameSystem
  {
    private Tile[,] _gameTiles;
    private TileTypeDictionary _tileDictionary;

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
          
      }
    }
  }
}
