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
    public class Model3DAvecCollision : Model3D
    {
        RessourcesManager<Model> GestionnaireDeMod�les { get; set; }
        Matrix[] TransformationMod�le { get; set; }
        Cam�ra Cam�raJeu { get; set; }
        float DeltaDivis�ParDeux { get; set; }
        public BoundingSphere SphereDeCollision { get; private set; }
        float Temps�coul�DepuisMAJ { get; set; }
        float Rayon { get; set; }

        public bool EstEnCollision(object autreObjet)
        {
            return Sph�reDeCollision.Intersects((autreObjet as Model3DAvecCollision).Sph�reDeCollision);
        }

        public BoundingSphere Sph�reDeCollision { get { return new BoundingSphere(Position, Rayon); } }


        public Model3DAvecCollision(Game game, string nomModele, float �chelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModele, �chelle, position, rotationInitiale)
        {
            
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            GestionnaireDeMod�les = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            DeltaDivis�ParDeux = (256 / 64f) / 2;
            Cam�raJeu = Game.Services.GetService(typeof(Cam�ra)) as Cam�ra;
            Mod�le = GestionnaireDeMod�les.Find(NomModele);
            SphereDeCollision = new BoundingSphere(Position, DeltaDivis�ParDeux);
            TransformationMod�le = new Matrix[Mod�le.Bones.Count];
            Mod�le.CopyAbsoluteBoneTransformsTo(TransformationMod�le);
            Rayon = 2f;
            CalculerMonde();
        }

        protected BoundingBox UpdateBoundingBox(Model model, Matrix worldTransform)
        {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), worldTransform);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }

            // Create and return bounding box
            return new BoundingBox(min, max);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            float temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps�coul�DepuisMAJ += temps�coul�;
            if (Temps�coul�DepuisMAJ >= (1f / 60f))
            {
                SphereDeCollision = new BoundingSphere(Position, DeltaDivis�ParDeux);
                Temps�coul�DepuisMAJ = 0;
            }
        }
        public override void Draw(GameTime gameTime)
        {
            foreach (ModelMesh maille in Mod�le.Meshes)
            {
                Matrix mondeLocal = TransformationMod�le[maille.ParentBone.Index] * GetMonde();
                foreach (ModelMeshPart portionDeMaillage in maille.MeshParts)
                {
                    BasicEffect effet = (BasicEffect)portionDeMaillage.Effect;
                    effet.EnableDefaultLighting();
                    effet.Projection = Cam�raJeu.Projection;
                    effet.View = Cam�raJeu.Vue;
                    effet.World = mondeLocal;
                }
                maille.Draw();
            }
        }

        private Matrix GetMonde()
        {
            return Monde;
        }
    }
}
