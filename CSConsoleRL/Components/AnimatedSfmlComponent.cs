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
    public class AnimatedSfmlComponent : IComponent
    {
        public Entity EntityAttachedTo { get; set; }
        public Sprite GameSprite { get; set; }
        public EnumSfmlSprites SpriteType { get; set; }

        public AnimatedSfmlComponent(Entity entity, EnumSfmlSprites spriteType)
        {
            EntityAttachedTo = entity;

            SpriteType = spriteType;
        }
        
        /// <summary>
        /// Component's update logic for next frame
        /// </summary>
        public virtual void NextFrame()
        {

        }
    }
}
