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
    public class Joueur : Model3D
    {
        string État { get; set; }
        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        float VitesseDéplacement { get; set; }
        float Angle { get; set; }
        InputManager GestionInput { get; set; }
        Caméra CaméraJeu { get; set; }
        MouseState GestionSouris { get; set; }

        public Joueur(Game game, string nomModele, float échelle, Vector3 position, Vector3 rotationInitiale, float intervalleMAJ)
            : base(game, nomModele, échelle, position, rotationInitiale)
        {
            IntervalleMAJ = intervalleMAJ;
        }

        public override void Initialize()
        {
            État = "enMouvement";
            VitesseDéplacement = 0.2f;
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            CaméraJeu = Game.Services.GetService(typeof(Caméra)) as Caméra;
            GestionSouris = Mouse.GetState(); //utilisé pour trouver la position de la souris
            base.Initialize();
        }

        // Va servir de state machine pour le joueur
        // Il va etre soit en mouvement, en construction ou mort
        public override void Update(GameTime gameTime)
        {
            switch (État)
            {
                case ("enMouvement"):
                    FaireMAJMouvement(gameTime);
                    break;
                case ("enConstruction"):
                    FaireMAJConstruction(gameTime);
                    break;
                case ("estMort"):
                    EstMort(gameTime);
                    break;
            }
        }

        //mise a jour pour les mouvements du jouer
        private void FaireMAJMouvement(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            GérerClavierMouvement();
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                GérerRotationJoueur();
                CalculerMonde();
                TempsÉcouléDepuisMAJ = 0;
            }
        }

        private Vector3 TrouverPositionSouris(MouseState ms)
        {
            Vector3 nearScreenPoint = new Vector3(ms.X, ms.Y, 0);
            Vector3 farScreenPoint = new Vector3(ms.X, ms.Y, 1);
            Vector3 nearWorldPoint = GraphicsDevice.Viewport.Unproject(nearScreenPoint, CaméraJeu.Projection, CaméraJeu.Vue, Matrix.Identity);
            Vector3 farWorldPoint = GraphicsDevice.Viewport.Unproject(farScreenPoint, CaméraJeu.Projection, CaméraJeu.Vue, Matrix.Identity);

            Vector3 direction = farWorldPoint - nearWorldPoint;

            float zFactor = -nearWorldPoint.Y / direction.Y;
            Vector3 zeroWorldPoint = nearWorldPoint + direction * zFactor;

            return zeroWorldPoint;
        }

        private void GérerRotationJoueur()
        {
            //Vector3 positionSouris = TrouverPositionSouris(GestionSouris);
            Point positionSouris = GestionInput.GetPositionSouris();
            Vector3 direction = new Vector3(positionSouris.X - Position.X, 0, positionSouris.Y - Position.Z);
            float distanceDirection = (float)Math.Sqrt(Math.Pow(direction.X, 2) + Math.Pow(direction.Y, 2) + Math.Pow(direction.Z,2));
            float vecteurBase = 2f;
            Angle = (float)Math.Acos(vecteurBase / distanceDirection);
            Rotation = new Vector3(0, Angle, 0);
        }

        private void GérerClavierMouvement()
        {
            if (GestionInput.EstEnfoncée(Keys.A))
            {
                Position += new Vector3(-VitesseDéplacement, 0, 0);
            }
            if (GestionInput.EstEnfoncée(Keys.D))
            {
                Position += new Vector3(VitesseDéplacement, 0, 0);
            }
            if (GestionInput.EstEnfoncée(Keys.W))
            {
                Position += new Vector3(0, 0, -VitesseDéplacement);
            }
            if (GestionInput.EstEnfoncée(Keys.S))
            {
                Position += new Vector3(0, 0, VitesseDéplacement);
            }


        }

        //mise a jour pour la construction
        private void FaireMAJConstruction(GameTime gameTime)
        {

        }

        //mise a jour pour la mort du joueur
        private void EstMort(GameTime gameTime)
        {

        }
    }
}
