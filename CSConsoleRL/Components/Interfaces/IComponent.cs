using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;

namespace CSConsoleRL.Components.Interfaces
{
    public interface IComponent
    {
        Entity EntityAttachedTo { get; set; }
        List<IComponent> SubscribedComponents { get; set; } //Let entities manage how components subscribe to each other (to avoid having to broadcast event to every component in an entity)
        void ReceiveComponentEvent(GameEvent componentEvent);
    }
}