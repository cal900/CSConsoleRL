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
  public class EntityAttackCoordsEvent : IGameEvent
  {
    public string EventName { get { return "EntityAttackCoords"; } }
    public List<object> EventParams { get; set; }

    public EntityAttackCoordsEvent(Entity entity, int baseDamage, int targetX, int targetY)
    {
      EventParams = new List<object>() { entity, baseDamage, targetX, targetY };
    }
  }
}
