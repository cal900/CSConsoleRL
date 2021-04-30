using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Entities;

namespace CSConsoleRL.Components
{
  public class HealthComponent : IComponent
  {
    public Entity EntityAttachedTo { get; set; }
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }

    public HealthComponent(Entity entity, int maxHealth)
    {
      EntityAttachedTo = entity;

      MaxHealth = maxHealth;
      CurrentHealth = maxHealth;
    }
  }
}
