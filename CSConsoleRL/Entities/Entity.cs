using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Events;
using CSConsoleRL.Components;

namespace CSConsoleRL.Entities
{
  public abstract class Entity
  {
    public Guid Id { get; set; }
    public Dictionary<Type, IComponent> Components { get; set; }

    public Entity()
    {
      Id = Guid.NewGuid();
      Components = new Dictionary<Type, IComponent>();
    }

    public bool HasComponent<T>()
    {
      IComponent temp;
      if (Components.TryGetValue(typeof(T), out temp) && temp != null)
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    public T GetComponent<T>()
    {
      IComponent temp;
      Components.TryGetValue(typeof(T), out temp);
      return (T)temp;
    }

    public void RemoveComponent<T>()
    {
      Components.Remove(typeof(T));
    }

    public void RemoveComponent(IComponent component)
    {
      var type = component.GetType();
      Components.Remove(type);
    }

    public Entity AddComponent(IComponent component)
    {
      var compType = component.GetType();

      if (component is IAiComponent)
      {
        compType = typeof(IAiComponent);
      }

      if (!Components.ContainsKey(compType))
      {
        Components.Add(compType, component);
        component.EntityAttachedTo = this;
      }
      else
      {
        throw new Exception("Trying to add a component to an Entity that already contains a component of that type");
      }

      return this;
    }

    public List<T> GetComponentsImplementing<T>() where T : class
    {
      return Components.Select(comp => comp.Value).OfType<T>().ToList();
    }

    protected bool Equals(Entity other)
    {
      return Id.Equals(other.Id);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((Entity)obj);
    }

    public override int GetHashCode()
    {
      return Id.GetHashCode();
    }
  }
}