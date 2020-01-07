using System;
using System.Collections.Generic;
using System.Text;

namespace CSConsoleRL.Data
{
  public enum EnumItemTypes
  {
    GateKey,
    Knife,
    Pistol,
    SniperRifle
  }

  public class Item
  {
    public EnumItemTypes ItemType;

    public Item(EnumItemTypes itemType)
    {
      ItemType = itemType;
    }
  }
}
