using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;
using SFML.System;

namespace CSConsoleRL.Components
{
    public class AiComponent : IComponent
    {
        public Entity EntityAttachedTo { get; set; }
        public int DesiredX { get; private set; }
        public int DesiredY { get; private set; }
        private List<Vector2i> _path;
        public List<Vector2i> Path
        {
            get
            {
                return _path;
            }
            set
            {
                if (value != null)
                {
                    _path = value;
                    if (_path.Count > 0)
                    {
                        DesiredX = _path[_path.Count - 1].X;
                        DesiredY = _path[_path.Count - 1].Y;
                    }
                }
            }
        }

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
