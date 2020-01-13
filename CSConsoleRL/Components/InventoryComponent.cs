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

    //Sets _activeItem to next item in list
    //Use for input to change _activeItem, and if removing _activeItem from _items
    //Wraps around to 0
    public void IncrementActiveItem()
    {
      if (_activeItem == null) return;
      var index = _items.IndexOf(_activeItem);

      if (index == _items.Count - 1)
      {
        _activeItem = _items[0];
      }
      else
      {
        _activeItem = _items[index + 1];
      }
    }

    public Item GetActiveItem()
    {
      return _activeItem;
    }

    public void AddItem(Item item)
    {
      _items.Add(item);
      if (_activeItem == null)
      {
        _activeItem = item;
      }
    }

    public void RemoveItem(Item item)
    {
      if (item == _activeItem)
      {
        IncrementActiveItem();
      }

      _items.Remove(item);
    }
  }
}
