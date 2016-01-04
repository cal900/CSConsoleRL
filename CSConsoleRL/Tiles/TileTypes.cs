using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Entities;

namespace CSConsoleRL.Tiles
{
    public struct GrassTile
    {
        public readonly char Character = ';';
        public readonly bool Blockable = false;
    }

    public struct SnowTile
    {
        public readonly char Character = '~';
        public readonly bool Blockable = false;
    }

    public struct SnowTileWalked
    {
        public readonly char Character = '8';
        public readonly bool Blockable = false;
    }

    public struct RoadTile
    {
        public readonly char Character = '[';
        public readonly bool Blockable = false;
    }
}
