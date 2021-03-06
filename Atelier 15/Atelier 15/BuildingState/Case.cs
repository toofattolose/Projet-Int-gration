﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtelierXNA
{
    public enum ÉtatCase { NonTesté, Ouverte, Fermée}

    public class Case
    {
        public Point Position { get; private set; }
        public bool Accessible { get; set; }
        public float G { get; set; } //Distance case-voisin
        public float H { get; set; } //Distance voisin-fin
        public float F { get { return G + H; } } //H+G
        public ÉtatCase État { get; set; }
        public Case CaseParent { get; set; }

        public Case(bool accessible, Point position)
        {
            Accessible = accessible;
            Position = position;
        }
        
    }
}
