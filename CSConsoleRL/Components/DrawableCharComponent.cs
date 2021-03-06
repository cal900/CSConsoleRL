﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;

namespace CSConsoleRL.Components
{
  public class DrawableCharComponent : IComponent
  {
    public Entity EntityAttachedTo { get; set; }
    public int XPositionOnMap { get; set; }
    public int YPositionOnMap { get; set; }
    public char Character { get; set; }
    public ConsoleColor CharColor { get; set; }

    public DrawableCharComponent(Entity entity, char character, ConsoleColor color)
    {
      EntityAttachedTo = entity;

      XPositionOnMap = 0;
      YPositionOnMap = 0;
      Character = character;
      CharColor = color;
    }
  }
}
