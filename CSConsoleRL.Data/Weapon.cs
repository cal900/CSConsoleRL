using System;
using System.Collections.Generic;
using System.Text;

namespace CSConsoleRL.Data
{
  public class Weapon : Item
  {
    public int Damage;
    public int Range;

    public Weapon(EnumItemTypes itemType, int damage = 0, int range = 0) : base(itemType)
    {
      Damage = damage;
      Range = range;

      AssignWeaponTypeValues();
    }

    private void AssignWeaponTypeValues()
    {
      if (Damage == 0)
      {
        switch (ItemType)
        {
          case EnumItemTypes.Knife:
            Damage = 25;
            break;
          case EnumItemTypes.Pistol:
            Damage = 50;
            break;
          case EnumItemTypes.SniperRifle:
            Damage = 75;
            break;
        }
      }

      if (Range == 0)
      {
        switch (ItemType)
        {
          case EnumItemTypes.Knife:
            Range = 1;
            break;
          case EnumItemTypes.Pistol:
            Range = 8;
            break;
          case EnumItemTypes.SniperRifle:
            Range = 20;
            break;
        }
      }
    }
  }
}
