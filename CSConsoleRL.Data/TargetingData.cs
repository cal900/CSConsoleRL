using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;

namespace CSConsoleRL.Data
{
  public class TargetingData
  {
    public readonly List<Vector2i> Path;
    public readonly Vector2i Target;

    public TargetingData(List<Vector2i> path, Vector2i target)
    {
      Path = path;
      Target = target;
    }
  }
}
