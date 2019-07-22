using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components;

namespace CSConsoleRL.Entities
{
    public class SeekerEntity : Entity
    {
        public SeekerEntity() 
            : base()
        {
            AddComponent(new PositionComponent(this));
            AddComponent(new DrawableSfmlComponent(this, Enums.EnumSfmlSprites.HumanEnemy));
            AddComponent(new UserInputComponent(this));
            AddComponent(new CollisionComponent(this));
            AddComponent(new SeekerAiComponent(this));
        }
    }
}
