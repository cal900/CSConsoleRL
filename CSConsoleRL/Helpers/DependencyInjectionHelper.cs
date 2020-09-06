using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using CSConsoleRL.Entities;
using CSConsoleRL.Components;

namespace CSConsoleRL.Helpers
{
  public static class DependencyInjectionHelper
  {
    private static readonly IDictionary<Type, object> _typeInstances = new Dictionary<Type, object>();

    public static void Register(object instance)
    {
      if (_typeInstances.ContainsKey(instance.GetType()))
      {
        throw new Exception(String.Format("DependencyInjectionHelper already contains type {0}", instance.GetType().ToString()));
      }

      _typeInstances.Add(instance.GetType(), instance);
    }

    public static object Resolve(Type type)
    {
      if (_typeInstances.ContainsKey(type))
      {
        return _typeInstances[type];
      }
      else
      {
        var constructor = type.GetConstructors()[0];
        var parameterInfos = constructor.GetParameters();

        var parameters = new List<object>();
        foreach (var parameterInfo in parameterInfos)
        {
          if (_typeInstances.ContainsKey(parameterInfo.ParameterType))
          {
            parameters.Add(_typeInstances[parameterInfo.ParameterType]);
          }
          else
          {
            throw new Exception(String.Format(
              "DependencyInjectionHelper was called to Resolve type {0}, but is missing type {1} which is required by Constructor",
              type.ToString(),
              parameterInfo.ParameterType.ToString()
              )
            );
          }
        }

        var typeInstance = constructor.Invoke(parameters.ToArray());
        Register(typeInstance);
        return typeInstance;
      }
    }
  }
}
