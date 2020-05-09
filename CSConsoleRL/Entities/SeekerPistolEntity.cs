﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components;
using CSConsoleRL.Data;

namespace CSConsoleRL.Entities
{
  public class SeekerPistolEntity : Entity
  {
    public SeekerPistolEntity()
        : base()
    {
      AddComponent(new PositionComponent(this));
      AddComponent(new DrawableSfmlComponent(this, Enums.EnumSfmlSprites.SeekerPistol));
      AddComponent(new CollisionComponent(this));
      AddComponent(new SeekerAiComponent(this));
      var inventory = new InventoryComponent(this);
      inventory.AddItem(new Item(EnumItemTypes.Pistol));
      AddComponent(inventory);
    }
  }
}
