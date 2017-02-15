using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtelierXNA
{
    class Case
    {
        public Point Position { get; private set; }
        public bool EstCaseVide { get; set; }
        public float DistanceCaseDépart { get; private set; }
        public float DistanceArrivée { get; private set; }
        public float DistanceTotale { get; set; }
        public ÉtatCase État { get; set; }
       // public Case CasePrécédente { get { } set { } }

        public void CalculerDistances()
        {
            DistanceArrivée = DistanceCaseDépart + DistanceArrivée;
        }

        
    }

    public enum ÉtatCase {Àtester, Ouverte, Fermée}
}
