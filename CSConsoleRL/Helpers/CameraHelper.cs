using System;
using System.Collections.Generic;
using System.Text;
using CSConsoleRL.Entities;
using CSConsoleRL.Components;

namespace CSConsoleRL.Helpers
{
  public sealed class CameraHelper
  {
    private Entity _entity;

    public CameraHelper(Entity entity)
    {
      //We can't snap the camera to an entity without a position...common man
      if (entity.HasComponent<PositionComponent>())
      {
        _entity = entity;
      }
      else
      {
        throw new Exception("CameraHelper was initialized with an entity that doesn't have a PositionComponent");
      }
    }

    public void SetEntity(Entity entity)
    {
      if (entity.HasComponent<PositionComponent>())
      {
        _entity = entity;
      }
      else
      {
        throw new Exception("Tried to set camera's entity to one that doesn't have a PositionComponent");
      }
    }

    public Entity GetEntity()
    {
      return _entity;
    }

    public int GetEntityXPositionOnMap()
    {
      return _entity.GetComponent<PositionComponent>().ComponentXPositionOnMap;
    }

    public int GetEntityYPositionOnMap()
    {
      return _entity.GetComponent<PositionComponent>().ComponentYPositionOnMap;
    }
  }
}
