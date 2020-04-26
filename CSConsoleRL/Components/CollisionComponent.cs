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
  public class CollisionComponent : IComponent
  {
    public Entity EntityAttachedTo { get; set; }

    public CollisionComponent(Entity entity)
    {
      EntityAttachedTo = entity;
    }
  }
}
