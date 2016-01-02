using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Entities;

namespace CSConsoleRL.Components
{
    public class DrawableCharComponent : IComponent
    {
        public Entity EntityAttachedTo { get; set; }
        public int XPositionOnMap { get; set; }
        public int YPositionOnMap { get; set; }
        public char Character { get; set; }

        public DrawableCharComponent(Entity entity, char character)
        {
            EntityAttachedTo = entity;
            XPositionOnMap = 0;
            YPositionOnMap = 0;
            Character = character;
        }
        
        public override void ReceiveEvent(string eventString)
        {
            switch(eventString)
            {
                case "TransformUp":
                    YPositionOnMap++;
                    return;
                case "TransformDown":
                    YPositionOnMap--;
                    return;
                case "TransformLeft":
                    XPositionOnMap--;
                    return;
                case "TransformRight":
                    XPositionOnMap++;
                    return;
                default:
                    return;
            }
        }

        public override void OnTick()
        {
            
        }
    }
}
