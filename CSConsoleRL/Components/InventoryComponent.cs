using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;
using CSConsoleRL.Data;

namespace CSConsoleRL.Components
{
  public class InventoryComponent : IComponent
  {
    private List<Item> _items;
    private Item _activeItem;
    public Entity EntityAttachedTo { get; set; }

    public InventoryComponent(Entity entity)
    {
      EntityAttachedTo = entity;

      _items = new List<Item>();
    }

    public Item GetActiveItem()
    {
      return _activeItem;
    }
  }
}
