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
    public class DrawableSfmlComponent : IComponent
    {
        public Entity EntityAttachedTo { get; set; }
        public int MyProperty { get; set; }

        public DrawableSfmlComponent(Entity entity, char character, ConsoleColor color)
        {
            EntityAttachedTo = entity;
            SubscribedComponents = new List<IComponent>();

            XPositionOnMap = 0;
            YPositionOnMap = 0;
            Character = character;
            CharColor = color;
        }

        public void ReceiveComponentEvent(GameEvent componentEvent)
        {
            throw new NotImplementedException();
        }
    }
}
