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
    public class AiComponent : IComponent
    {
        public Entity EntityAttachedTo { get; set; }
        public int DesiredX { get; set; }
        public int DesiredY { get; set; }

        public AiComponent(Entity entity)
        {
            EntityAttachedTo = entity;
        }
    }

    public class SeekerAiComponent: AiComponent
    {
        public SeekerAiComponent(Entity entity) : base(entity) { }
    }
}
