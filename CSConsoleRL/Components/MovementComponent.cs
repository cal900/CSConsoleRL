using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;

namespace CSConsoleRL.Components
{
    public class MovementComponent : IComponent
    {
        public Entity EntityAttachedTo { get; set; }
        public List<IComponent> SubscribedComponents { get; set; }
        public int ComponentXPositionOnMap { get; set; }
        public int ComponentYPositionOnMap { get; set; }

        public MovementComponent(Entity entity)
        {
            EntityAttachedTo = entity;
            SubscribedComponents = new List<IComponent>();
            ComponentXPositionOnMap = 0;
            ComponentYPositionOnMap = 0;
        }

        public void ReceiveComponentEvent(GameEvent componentEvent)
        {
            if(componentEvent.EventName == "MovementCommand")
            {
                MovementCommandEvent newEvent = (MovementCommandEvent)componentEvent;
                for(int index = 0; index < SubscribedComponents.Count; index++)
                {
                    SubscribedComponents[index].ReceiveComponentEvent(newEvent);
                }
            }
        }
    }
}
