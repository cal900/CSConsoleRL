using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;

namespace CSConsoleRL.Components.Interfaces
{
    public interface IComponent
    {
        Entity EntityAttachedTo { get; set; }
    }
}