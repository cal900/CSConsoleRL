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
  public class EntityRequestAttackEvent : IGameEvent
  {
    public string EventName { get { return "EntityRequestAttack"; } }
    public List<object> EventParams { get; set; }

    public EntityRequestAttackEvent(Entity entity, int targetX, int targetY)
    {
      EventParams = new List<object>() { entity, targetX, targetY };
    }
  }
}
