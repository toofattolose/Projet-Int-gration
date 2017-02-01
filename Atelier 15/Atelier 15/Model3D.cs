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
    ///SUPA HOT FIRE
    
    /// <summary>
    /// C'est le model de base pour tous les modèles 3D
    /// </summary>
    public class Model3D : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Vector3 Position { get; set; }
        protected Vector3 Rotation { get; set; }
        protected float Échelle { get; set; }
        string NomModele { get; set; }
        RessourcesManager<Model> GestionnaireDeModèles { get; set; }
        protected Matrix Monde { get; set; }
        Model Modèle { get; set; }
        Matrix[] TransformationModèle { get; set; }
        Caméra CaméraJeu { get; set; }

        public Model3D(Game game, string nomModele, float échelle, Vector3 position, Vector3 rotationInitiale)
            : base(game)
        {
            Position = position;
            Rotation = rotationInitiale;
            Échelle = échelle;
            NomModele = nomModele;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            GestionnaireDeModèles = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            CaméraJeu = Game.Services.GetService(typeof(Caméra)) as Caméra;
            Modèle = GestionnaireDeModèles.Find(NomModele);
            TransformationModèle = new Matrix[Modèle.Bones.Count];
            Modèle.CopyAbsoluteBoneTransformsTo(TransformationModèle);
            CalculerMonde();
        }

        protected void CalculerMonde()
        {
            Monde = Matrix.Identity;
            Monde *= Matrix.CreateScale(Échelle);
            Monde *= Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
            Monde *= Matrix.CreateTranslation(Position);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (ModelMesh maille in Modèle.Meshes)
            {
                Matrix mondeLocal = TransformationModèle[maille.ParentBone.Index] * GetMonde();
                foreach (ModelMeshPart portionDeMaillage in maille.MeshParts)
                {
                    BasicEffect effet = (BasicEffect)portionDeMaillage.Effect;
                    effet.EnableDefaultLighting();
                    effet.Projection = CaméraJeu.Projection;
                    effet.View = CaméraJeu.Vue;
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
}//slave 2/
