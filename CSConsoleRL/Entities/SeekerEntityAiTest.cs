using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components;
using CSConsoleRL.Data;

namespace CSConsoleRL.Entities
{
  public class SeekerAiTestEntity : Entity
  {
    public override string Name { get; protected set; }

    public SeekerAiTestEntity()
        : base()
    {
      Name = "SeekerAiTest";
      AddComponent(new PositionComponent(this));
      AddComponent(new DrawableSfmlComponent(this, Enums.EnumSfmlSprites.Seeker));
      AddComponent(new CollisionComponent(this));
      AddComponent(new AiTestComponent(this));
      AddComponent(new InventoryComponent(this));
      this.GetComponent<InventoryComponent>().AddItem(new Weapon(EnumItemTypes.Knife));
      AddComponent(new HealthComponent(this, 100));

    }
  }
}
