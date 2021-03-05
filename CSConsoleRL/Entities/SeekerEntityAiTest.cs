using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components;

namespace CSConsoleRL.Entities
{
  public class SeekerAiTestEntity : Entity
  {
    public SeekerAiTestEntity()
        : base()
    {
      AddComponent(new PositionComponent(this));
      AddComponent(new DrawableSfmlComponent(this, Enums.EnumSfmlSprites.Seeker));
      AddComponent(new CollisionComponent(this));
      AddComponent(new AiTestComponent(this));
    }
  }
}
