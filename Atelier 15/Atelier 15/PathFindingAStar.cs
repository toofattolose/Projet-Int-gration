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
    public class PathFindingAStar : Microsoft.Xna.Framework.GameComponent
    {
        public Point PositionDÈpart { get; set; }
        public Point PositionFinale { get; set; }
        public bool[,] Map { get; set; }
        GridDeJeu Grid { get; set; }


        public PathFindingAStar(Game game):base(game)
        {
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            Grid = Game.Services.GetService(typeof(GridDeJeu)) as GridDeJeu;
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        // …TABLIR PATH POINT A AU POINT PLAYER
        // CR…ER TOUTES LES CASES (TABLEAU 2D)

        //private bool ChercherCase¿Aller(Case case1)
        //{
        //    List<Case> casesAdjacentes = TrouverCasesAdjacentesVides(case1);
        //    casesAdjacentes.OrderBy(x => x.DistanceTotale).ToList();
        //    foreach (Case caseAdjacente in casesAdjacentes)
        //    {
        //    }
        //}

        //private List<Case> TrouverCasesAdjacentesVides(Case case1)
        //{
        //    List<Case> casesVides = new List<Case>();
        //    List<Point> positionsAdjacentes = TrouverPointsAdjacents(case1.Position);
        //}

        private List<Point> TrouverPointsAdjacents(Point position)
        {
            List<Point> positionsAdjacentes = new List<Point>();

            for(int i = -1; i <= 1; ++i)
            {
                for(int j = -1; j <= 1; ++j)
                {
                    if(i == 0 && j == 0)
                    {
                        ++j;
                    }

                    //if() G…RER BORDURES
                    positionsAdjacentes.Add(new Point(position.X + i, position.Y + j));
                }
            }

            return positionsAdjacentes;
        }

    }
}
