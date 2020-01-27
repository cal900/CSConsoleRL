using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Enums;
using CSConsoleRL.Entities;
using CSConsoleRL.Data;

namespace CSConsoleRL.Events
{
  public class ChangeActiveItemEvent : IGameEvent
  {
    public string EventName { get { return "ChangeActiveItem"; } }
    public List<object> EventParams { get; set; }

    /// <summary>
    /// Changes the active item to the next item in inventory
    /// </summary>
    /// <param name="entity"></param>
    public ChangeActiveItemEvent(Entity entity)
    {
      EventParams = new List<object>() { entity };
    }

    /// <summary>
    /// Changes the active item to the specified one
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="newActiveItem"></param>
    public ChangeActiveItemEvent(Entity entity, Item newActiveItem)
    {
      EventParams = new List<object>() { entity, newActiveItem };
    }
  }
}
