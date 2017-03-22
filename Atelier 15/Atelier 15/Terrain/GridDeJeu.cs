using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace AtelierXNA
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GridDeJeu : Microsoft.Xna.Framework.GameComponent
    {
        Vector3 Étendue { get; set; }
        public Vector2 Charpente { get; private set; }
        public Vector2 Delta { get; private set; }
        public bool[,] TableauGrid { get; set; }
        public Case[,] GridCase { get; set; }
        public float DeltaDiviséParDeux { get; private set; }


        //recoit en parametre l'étendu et la charpente (grandeur et dimension du grid)
        public GridDeJeu(Game game, Vector3 étendue, Vector2 charpente)
            : base(game)
        {
            Étendue = étendue;
            Charpente = charpente;
            TableauGrid = new bool[(int)Charpente.X, (int)Charpente.Y];
            GridCase = new Case[(int)Charpente.X, (int)Charpente.Y];
            for (int i = 0; i < Charpente.X; i++)
            {
                for (int j = 0; j < Charpente.Y; j++)
                {
                    TableauGrid[i, j] = true;
                    GridCase[i, j] = new Case(true, new Point(i, j));
                }
            }
            Delta = new Vector2(Étendue.X / Charpente.X, Étendue.Z / Charpente.Y);
            DeltaDiviséParDeux = Delta.X / 2;
        }

        //fonction pathfinding
        public List<Case> GetVoisins(Case caseActuelle)
        {
            List<Case> ListVoisins = new List<Case>();
            for(int i = 1; i > -2; --i)
            {
                for (int j = -1; j < 2; ++j)
                {
                    if (!(j == 0 && i == 0))
                    {
                        if(caseActuelle.Position.X + j >= 0 && caseActuelle.Position.X + j < 64 && caseActuelle.Position.Y + i >= 0 && caseActuelle.Position.Y + i < 64)
                        {
                            ListVoisins.Add(GridCase[caseActuelle.Position.X + j, caseActuelle.Position.Y + i]);
                        }
                    }
                }
            }

            return ListVoisins;
        }
    }
}
