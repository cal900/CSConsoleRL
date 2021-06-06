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
  public class PlayerRequestAttackEvent : IGameEvent
  {
    public string EventName { get { return "PlayerRequestAttack"; } }
    public List<object> EventParams { get; set; }

    public PlayerRequestAttackEvent()
    {
      EventParams = new List<object>() { };
    }
  }
}
