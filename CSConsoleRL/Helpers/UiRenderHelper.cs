using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using GameTiles.Tiles;
using CSConsoleRL.Logging;

namespace CSConsoleRL.Helpers
{
  public sealed class UiRenderHelper
  {
    private static UiRenderHelper _instance;
    public static UiRenderHelper Instance
    {
      get
      {
        if (_instance == null)
        {
          _instance = new UiRenderHelper();
        }
        return _instance;
      }
    }

    private UiRenderHelper()
    {

    }


  }
}
