using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components;
using CSConsoleRL.Data;

namespace CSConsoleRL.Entities
{
  public class SeekerKnifeEntity : Entity
  {
    public override string Name { get; protected set; }

    public SeekerKnifeEntity()
        : base()
    {
      Name = "SeekerKnife";
      AddComponent(new PositionComponent(this));
      AddComponent(new DrawableSfmlComponent(this, Enums.EnumSfmlSprites.SeekerKnife));
      AddComponent(new CollisionComponent(this));
      AddComponent(new SeekerAiComponent(this));
      var inventory = new InventoryComponent(this);
      inventory.AddItem(new Item(EnumItemTypes.Knife));
      AddComponent(inventory);
    }
  }
}
