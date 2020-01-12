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
  public class SendActiveItemEvent : IGameEvent
  {
    public string EventName { get { return "SendActiveItem"; } }
    public List<object> EventParams { get; set; }

    public SendActiveItemEvent(Item activeItem)
    {
      EventParams = new List<object>() { activeItem };
    }
  }
}
