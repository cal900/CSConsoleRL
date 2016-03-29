using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Events;

namespace CSConsoleRL.Entities
{
    public class Entity
        {
            public Guid Id { get; set; }
            public Dictionary<Type, IComponent> Components { get; set; }

            public Entity()
            {
                Id = Guid.NewGuid();
                Components = new Dictionary<Type, IComponent>();
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
                if (!Components.ContainsKey(component.GetType()))
                {
                    Components.Add(component.GetType(), component);
                    component.EntityAttachedTo = this;
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

            public GameEvent BroadcastEvent(GameEvent evnt)
            {
                foreach(KeyValuePair<Type, IComponent> currentComponent in Components)
                {
                    currentComponent.Value.ReceiveComponentEvent(evnt);
                }
            }
        }
    }