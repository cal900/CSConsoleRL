using System;
using System.Collections.Generic;
using System.Text;

namespace CSConsoleRL.Data
{
  public class Weapon : Item
  {
    public int Range;

    public Weapon(EnumItemTypes itemType, int range) : base(itemType)
    {
      ItemType = itemType;
      Range = range;
    }
  }
}
