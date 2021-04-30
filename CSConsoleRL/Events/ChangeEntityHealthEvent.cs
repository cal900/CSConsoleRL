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
  public class ChangeEntityHealthEvent : IGameEvent
  {
    public string EventName { get { return "ChangeEntityHealth"; } }
    public List<object> EventParams { get; set; }

    public ChangeEntityHealthEvent(Guid id, int damage)
    {
      EventParams = new List<object>() { id, damage };
    }
  }
}
