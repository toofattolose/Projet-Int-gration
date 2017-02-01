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
    /// C'est le model de base pour tous les mod�les 3D
    /// </summary>
    public class Model3D : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Vector3 Position { get; set; }
        protected Vector3 Rotation { get; set; }
        protected float �chelle { get; set; }
        string NomModele { get; set; }
        RessourcesManager<Model> GestionnaireDeMod�les { get; set; }
        protected Matrix Monde { get; set; }
        Model Mod�le { get; set; }
        Matrix[] TransformationMod�le { get; set; }
        Cam�ra Cam�raJeu { get; set; }

        public Model3D(Game game, string nomModele, float �chelle, Vector3 position, Vector3 rotationInitiale)
            : base(game)
        {
            Position = position;
            Rotation = rotationInitiale;
            �chelle = �chelle;
            NomModele = nomModele;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            GestionnaireDeMod�les = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            Cam�raJeu = Game.Services.GetService(typeof(Cam�ra)) as Cam�ra;
            Mod�le = GestionnaireDeMod�les.Find(NomModele);
            TransformationMod�le = new Matrix[Mod�le.Bones.Count];
            Mod�le.CopyAbsoluteBoneTransformsTo(TransformationMod�le);
            CalculerMonde();
        }

        protected void CalculerMonde()
        {
            Monde = Matrix.Identity;
            Monde *= Matrix.CreateScale(�chelle);
            Monde *= Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
            Monde *= Matrix.CreateTranslation(Position);
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
}//slave 2/
