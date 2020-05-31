using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;

namespace CSConsoleRL.Data
{
  public class TargetingSquare
  {
    public readonly Vector2i _coords;
    public readonly bool _valid;

    public TargetingSquare(Vector2i coords, bool valid)
    {
      _coords = coords;
      _valid = valid;
    }
  }
}
