using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Entities;

namespace CSConsoleRL.Tiles
{
    public class BaseTile : Entity
    {
        public Dictionary<Type, IComponent> Components { get; set; }

        public BaseTile()
            : base()
        {

        }
    }
}
