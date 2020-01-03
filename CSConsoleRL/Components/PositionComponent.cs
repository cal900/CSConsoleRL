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
  public class PositionComponent : IComponent
  {
    public Entity EntityAttachedTo { get; set; }
    public int ComponentXPositionOnMap { get; set; }
    public int ComponentYPositionOnMap { get; set; }

    public PositionComponent(Entity entity)
    {
      EntityAttachedTo = entity;
      ComponentXPositionOnMap = 15;
      ComponentYPositionOnMap = 15;
    }
  }
}
