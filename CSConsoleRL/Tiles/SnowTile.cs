using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components;

namespace CSConsoleRL.Tiles
{
    public class SnowTile : BaseTile
    {
        private bool bootTracks;
        public bool BootTracks
        {
            get { return bootTracks; }
            set { SetBookTracks(value); }
        }

        public SnowTile()
            : base()
        {
            AddComponent(new DrawableCharComponent(this, '~', ConsoleColor.White));
        }

        private void SetBookTracks(bool value)
        {
            bootTracks = value;

            if (bootTracks)
            {
                GetComponent<DrawableCharComponent>().Character = '8';
            }
            else
            {
                GetComponent<DrawableCharComponent>().Character = '~';
            }
        }
    }
}
