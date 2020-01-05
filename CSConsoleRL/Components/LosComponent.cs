﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;

namespace CSConsoleRL.Components
{
    public class InventoryComponent : IComponent
    {
        public Entity EntityAttachedTo { get; set; }

        public InventoryComponent(Entity entity)
        {
            EntityAttachedTo = entity;
        }
    }
}
