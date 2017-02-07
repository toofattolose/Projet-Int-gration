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
    public class GridPathFinding : Microsoft.Xna.Framework.GameComponent
    {
        public Point PositionDÈpart { get; set; }
        public Point PositionFinale { get; set; }
        bool[,] Map { get; set; }
        GridDeJeu Grid { get; set; }


        public GridPathFinding(Game game):base(game)
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

        //private bool ChercherCase¿Aller(Case positionCase)
        //{
        //    List<Case> casesAdjacentes = TrouverCasesAdjacentesVides(positionCase);
        //    casesAdjacentes.Sort((case1, case2) => case1.DistanceTotale.CompareTo(case2.DistanceTotale));
        //    foreach (Case caseAdjacente in casesAdjacentes)
        //    {
        //    }
        //}

        //private List<Case> TrouverCasesAdjacentesVides(Case positionCase)
        //{
            
        //}

    }
}
