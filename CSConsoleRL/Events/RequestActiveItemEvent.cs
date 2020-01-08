using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Enums;
using CSConsoleRL.Entities;

namespace CSConsoleRL.Events
{
  public class RequestActiveItemEvent : IGameEvent
  {
    public string EventName { get { return "RequestActiveItem"; } }
    public List<object> EventParams { get; set; }

    public RequestActiveItemEvent(Guid id)
    {
      EventParams = new List<object>() { id };
    }
  }
}
