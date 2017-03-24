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
    public class CaseDeConstruction : DrawableGameComponent
    {
        Color Couleur { get; set; }
        Vector3[,] PtsSommets { get; set; }
        Vector3 PositionInitiale { get; set; }
        VertexPositionColor[] Sommets { get; set; }
        GridDeJeu Grid { get; set; }
        BasicEffect EffetDeBase { get; set; }
        public Vector2 PositionDansGrid { get; set; }
        Caméra CaméraJeu { get; set; }
        int NbTriangles { get; set; }

        public CaseDeConstruction(Game game, Vector3 positionInitiale)
            : base(game)
        {
            PositionInitiale = positionInitiale;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            PtsSommets = new Vector3[2, 2];
            Sommets = new VertexPositionColor[4];
            Grid = Game.Services.GetService(typeof(GridDeJeu)) as GridDeJeu;
            CaméraJeu = Game.Services.GetService(typeof(Caméra)) as Caméra;
            NbTriangles = 2;
            GénérerPoints();
            EffetDeBase = new BasicEffect(Game.GraphicsDevice);
            InitialiserParamètresEffetDeBase();
            PositionDansGrid = new Vector2((float)Math.Floor(PositionInitiale.X / Grid.Delta.X), (float)Math.Floor(PositionInitiale.Z / Grid.Delta.Y));
            if (Grid.TableauGrid[(int)PositionDansGrid.X, (int)PositionDansGrid.Y])
            {
                Couleur = Color.Green;
            }
            else
            {
                Couleur = Color.Red;
            }
            InitialiserSommets();
            Visible = false;
        }

        void InitialiserParamètresEffetDeBase()
        {
            EffetDeBase.VertexColorEnabled = true;
        }

        private void GénérerPoints()
        {
            int ptsData = 0;

            for (int i = 0; i < PtsSommets.GetLength(0); ++i)
            {
                for (int j = PtsSommets.GetLength(1) - 1; j >= 0; --j)
                {
                    PtsSommets[i, j] = new Vector3(PositionInitiale.X + (i * Grid.Delta.X), 0.25f, PositionInitiale.Z + (j * Grid.Delta.Y));
                    ++ptsData;
                }
            }
        }

        protected void InitialiserSommets()
        {
            int noSommets = -1;

            for (int j = 0; j < PtsSommets.GetLength(1) - 1; ++j)
            {
                for (int i = 0; i < PtsSommets.GetLength(0) - 1; ++i)
                {

                    Sommets[++noSommets] = new VertexPositionColor(PtsSommets[i, j + 1], Couleur);
                    Sommets[++noSommets] = new VertexPositionColor(PtsSommets[i, j], Couleur);
                    Sommets[++noSommets] = new VertexPositionColor(PtsSommets[i + 1, j + 1], Couleur);
                    Sommets[++noSommets] = new VertexPositionColor(PtsSommets[i + 1, j], Couleur);
                }
            }

        }

        public override void Update(GameTime gameTime)
        {
            if (Grid.TableauGrid[(int)PositionDansGrid.X, (int)PositionDansGrid.Y])
            {
                Couleur = Color.Green;
            }
            else
            {
                Couleur = Color.Red;
            }
            InitialiserSommets();
        }

        public override void Draw(GameTime gameTime)
        {
            EffetDeBase.World = Matrix.Identity;
            EffetDeBase.View = CaméraJeu.Vue;
            EffetDeBase.Projection = CaméraJeu.Projection;
            foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
            {
                passeEffet.Apply();
                Game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, Sommets, 0, NbTriangles);
            }
        }
    }
}
