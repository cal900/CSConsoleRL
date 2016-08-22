﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components;

namespace CSConsoleRL.Entities
{
    //Meant to be used for player character
    public class ActorEntity : Entity
    {
        public ActorEntity() 
            : base()
        {
            AddComponent(new PositionComponent(this));
            AddComponent(new DrawableCharComponent(this, '@', ConsoleColor.Gray));
            AddComponent(new UserInputComponent(this));
            AddComponent(new CollisionComponent(this));
        }
    }
}
