using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;
using CSConsoleRL.Enums;
using SFML.Graphics;

namespace CSConsoleRL.Components
{
    public class FadingSfmlComponent : AnimatedSfmlComponent, IComponent
    {
        public FadingSfmlComponent(Entity entity, EnumSfmlSprites spriteType)
            : base(entity, spriteType)
        {
            EntityAttachedTo = entity;

            SpriteType = spriteType;
        }

        public override void NextFrame()
        {
            var currentColor = GameSprite.Color;

            if (currentColor.A > 0) currentColor.A--;

            GameSprite.Color = currentColor;
        }

        public bool ShouldDelete()
        {
            return GameSprite.Color.A == 0 ? true : false;
        }
    }
}
