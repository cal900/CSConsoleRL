using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;
using CSConsoleRL.Ai;
using CSConsoleRL.Ai.Interfaces;
using CSConsoleRL.Helpers;

namespace CSConsoleRL.Components
{
  public interface IAiComponent : IComponent
  {
    public IGameEvent GetAiComponentResponse(GameStateHelper gameStateHelper);
  }

  public class AiComponent : IAiComponent
  {
    public Entity EntityAttachedTo { get; set; }
    private readonly IAi _ai;
    public AiComponent(Entity entity, IAi ai)
    {
      EntityAttachedTo = entity;
      _ai = ai;
    }

    public IGameEvent GetAiComponentResponse(GameStateHelper gameStateHelper)
    {
      return _ai.GetAiResponse(gameStateHelper);
    }
  }
}
