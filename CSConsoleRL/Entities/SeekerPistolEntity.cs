﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components;
using CSConsoleRL.Data;
using CSConsoleRL.Ai;

namespace CSConsoleRL.Entities
{
  public class SeekerPistolEntity : Entity
  {
    public override string Name { get; protected set; }

    public SeekerPistolEntity()
        : base()
    {
      Name = "SeekerPistol";
      AddComponent(new PositionComponent(this));
      AddComponent(new DrawableSfmlComponent(this, Enums.EnumSfmlSprites.SeekerPistol));
      AddComponent(new CollisionComponent(this));
      AddComponent(new AiComponent(this, new AiGuard(this)));
      var inventory = new InventoryComponent(this);
      inventory.AddItem(new Item(EnumItemTypes.Pistol));
      AddComponent(inventory);
    }
  }
}
