using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components;

namespace CSConsoleRL.Entities
{
  //Meant for debugging, should color in square with solid color then fade out
  public class FadingColorEntity : Entity
  {
    public override string Name { get; protected set; }

    public FadingColorEntity(string color)
        : base()
    {
      Name = "FadingColor";
      AddComponent(new PositionComponent(this));

      if (color == null) color = "";
      switch (color)
      {
        case "green":
          AddComponent(new FadingSfmlComponent(this, Enums.EnumSfmlSprites.GreenSquare));
          break;
        case "yellow":
          AddComponent(new FadingSfmlComponent(this, Enums.EnumSfmlSprites.YellowSquare));
          break;
        default:
          AddComponent(new FadingSfmlComponent(this, Enums.EnumSfmlSprites.GreenSquare));
          break;
      }
    }
  }
}
