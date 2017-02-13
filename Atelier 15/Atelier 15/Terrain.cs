using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
    public class Terrain : PrimitiveDeBaseAnimée
    {
        const int NB_TRIANGLES_PAR_TUILE = 2;
        const int NB_SOMMETS_PAR_TRIANGLE = 3;
        const float MAX_COULEUR = 255f;

        Vector3 Étendue { get; set; }
        float DeltaY { get; set; }
        int NbTriangle { get; set; }
        Vector2 Charpente { get; set; }
        Vector3[,] PtsSommets { get; set; }
        VertexPositionColor[] Sommets { get; set; }
        Vector2 Delta { get; set; }
        BasicEffect EffetDeBase { get; set; }
        Vector3 Origine { get; set; }

        // à compléter en ajoutant les propriétés qui vous seront nécessaires pour l'implémentation du composant


        public Terrain(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale,
                       Vector3 étendue, Vector2 charpente, float intervalleMAJ)
           : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
        {
            Étendue = étendue;
            Charpente = charpente;
        }

        public override void Initialize()
        {
            InitialiserDonnéesCarte();
            //Origine = new Vector3(-Étendue.X / 2, 0, Étendue.Z / 2); 
            Origine = Vector3.Zero;
            Delta = new Vector2(Étendue.X / Charpente.X, Étendue.Z / Charpente.Y);
            AllouerTableaux();
            CréerTableauPoints();
            base.Initialize();
        }

        void InitialiserDonnéesCarte()
        {
            NbSommets = (int)(Charpente.X * Charpente.Y) * NB_SOMMETS_PAR_TRIANGLE * NB_TRIANGLES_PAR_TUILE;
            NbTriangles = ((int)Charpente.X * (int)Charpente.Y) * 2;
        }

        void AllouerTableaux()
        {
            PtsSommets = new Vector3[(int)Charpente.X , (int)Charpente.Y];
            Sommets = new VertexPositionColor[NbSommets];
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            EffetDeBase = new BasicEffect(GraphicsDevice);
            InitialiserParamètresEffetDeBase();
        }

        void InitialiserParamètresEffetDeBase()
        {
            EffetDeBase.VertexColorEnabled = true;
        }

        private void CréerTableauPoints()
        {
            int ptsData = 0;

            for (int i = 0; i < PtsSommets.GetLength(0); ++i)
            {
                for (int j = PtsSommets.GetLength(1)-1;j >=0; --j)
                {
                    PtsSommets[i, j] = new Vector3((Origine.X + i * Delta.X), 0,(Origine.Y + j * Delta.Y));
                    ++ptsData;
                }
            }
        }

        protected override void InitialiserSommets()
        {
            int noSommets = -1;

            for(int j = 0; j < Charpente.Y - 1; ++j)
            {
                for(int i = 0; i < Charpente.X - 1; ++i)
                {
                    Sommets[++noSommets] = new VertexPositionColor(PtsSommets[i, j + 1], Color.DarkGreen);
                    Sommets[++noSommets] = new VertexPositionColor(PtsSommets[i, j], Color.DarkGreen);
                    Sommets[++noSommets] = new VertexPositionColor(PtsSommets[i + 1, j], Color.DarkGreen);
                    Sommets[++noSommets] = new VertexPositionColor(PtsSommets[i, j + 1], Color.DarkGreen);
                    Sommets[++noSommets] = new VertexPositionColor(PtsSommets[i + 1, j], Color.DarkGreen);
                    Sommets[++noSommets] = new VertexPositionColor(PtsSommets[i + 1, j + 1], Color.DarkGreen);
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            EffetDeBase.World = GetMonde();
            EffetDeBase.View = CaméraJeu.Vue;
            EffetDeBase.Projection = CaméraJeu.Projection;
            foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
            {
                passeEffet.Apply();
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Sommets, 0, NbTriangles);
            }
        }
    }
}
