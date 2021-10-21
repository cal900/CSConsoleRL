using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components;

namespace CSConsoleRL.Entities
{
  public class XMarkerEntity : Entity
  {
    public override string Name { get; protected set; }

    public XMarkerEntity()
        : base()
    {
      Name = "XMarker";
      AddComponent(new PositionComponent(this));
      AddComponent(new DrawableSfmlComponent(this, Enums.EnumSfmlSprites.RedX));
    }
  }
}
