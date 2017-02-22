using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace AtelierXNA
{
    public class JoueurConnection
    {
        int score_;
        string nom_;
        public NetConnection Connection { get; set; }

        public int Score
        {
            get
            {
                return score_;
            }
            set
            {
                score_ = value;
            }
        }

        public string Nom
        {
            get
            {
                return nom_;
            }
            private set
            {
                nom_ = value;
            }
        }


        public JoueurConnection(string nom, NetConnection net)
        {
            Nom = nom;
            Connection = net;
            Score = 0;
        }
    }
}
