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
        string �tat { get; set; }
        float IntervalleMAJ { get; set; }
        float Temps�coul�DepuisMAJ { get; set; }
        float VitesseD�placement { get; set; }
        float Angle { get; set; }
        InputManager GestionInput { get; set; }
        Cam�ra Cam�raJeu { get; set; }
        MouseState GestionSouris { get; set; }
        RessourcesManager<Model> GestionnaireDeMod�les { get; set; }
        Model Roche { get; set; }
        Model Arbre { get; set; }
        Model Or { get; set; }

        public Joueur(Game game, string nomModele, float �chelle, Vector3 position, Vector3 rotationInitiale, float intervalleMAJ)
            : base(game, nomModele, �chelle, position, rotationInitiale)
        {
            IntervalleMAJ = intervalleMAJ;
        }

        public override void Initialize()
        {
            �tat = "enMouvement";
            VitesseD�placement = 0.2f;
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GestionnaireDeMod�les = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            Roche = GestionnaireDeMod�les.Find("rock1");
            Or = GestionnaireDeMod�les.Find("gold1");
            Arbre = GestionnaireDeMod�les.Find("tree1");
            Cam�raJeu = Game.Services.GetService(typeof(Cam�ra)) as Cam�ra3rdPerson;
            GestionSouris = Mouse.GetState(); //utilis� pour trouver la position de la souris
            base.Initialize();
        }

        // Va servir de state machine pour le joueur
        // Il va etre soit en mouvement, en construction ou mort
        public override void Update(GameTime gameTime)
        {
            switch (�tat)
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
            float temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps�coul�DepuisMAJ += temps�coul�;
            G�rerClavierMouvement();
            if (Temps�coul�DepuisMAJ >= IntervalleMAJ)
            {
                G�rerRotationJoueur();
                G�rerPicking();
                Cam�raJeu.D�placer(Position);
                CalculerMonde();
                Temps�coul�DepuisMAJ = 0;
            }
        }

        //Utilis� pour trouver la position de la souris dans un environnement 3D
        private Vector3 TrouverPositionSouris(Point ms)
        {
            Vector2 positionSouris = new Vector2(ms.X, ms.Y);
            Vector3 nearScreenPoint = new Vector3(positionSouris, 0);
            Vector3 farScreenPoint = new Vector3(positionSouris, 1.01f);
            Vector3 nearWorldPoint = Game.GraphicsDevice.Viewport.Unproject(nearScreenPoint, Cam�raJeu.Projection, Cam�raJeu.Vue, Matrix.Identity);
            Vector3 farWorldPoint = Game.GraphicsDevice.Viewport.Unproject(farScreenPoint, Cam�raJeu.Projection, Cam�raJeu.Vue, Matrix.Identity);

            Vector3 direction = farWorldPoint - nearWorldPoint;

            float zFactor = -nearWorldPoint.Y / direction.Y;
            Vector3 zeroWorldPoint = nearWorldPoint + direction * zFactor;

            return zeroWorldPoint;
        }
        private Ray TrouverPositionSourisPicking(Point ms)
        {
            Vector2 positionSouris = new Vector2(ms.X, ms.Y);
            Vector3 nearScreenPoint = new Vector3(positionSouris, 0);
            Vector3 farScreenPoint = new Vector3(positionSouris, 1.01f);
            Vector3 nearWorldPoint = Game.GraphicsDevice.Viewport.Unproject(nearScreenPoint, Cam�raJeu.Projection, Cam�raJeu.Vue, Matrix.Identity);
            Vector3 farWorldPoint = Game.GraphicsDevice.Viewport.Unproject(farScreenPoint, Cam�raJeu.Projection, Cam�raJeu.Vue, Matrix.Identity);

            Vector3 direction = farWorldPoint - nearWorldPoint;
            direction.Normalize();
            return new Ray(nearScreenPoint, direction);
        }
        private void G�rerRotationJoueur()
        {
            Point positionSourisInitiale = GestionInput.GetPositionSouris();
            Vector3 positionSouris = TrouverPositionSouris(positionSourisInitiale);
            Vector3 direction = new Vector3(positionSouris.X - Position.X, 0, positionSouris.Z - Position.Z);
            Vector3 directionBase = Vector3.UnitX;
            direction.Normalize();
            directionBase.Normalize();
            double cosAngle = Vector3.Dot(direction, directionBase);
            if (positionSouris.Z > Position.Z)
            {
                Angle = -(float)Math.Acos(cosAngle);
            }
            else
            {
                Angle = (float)Math.Acos(cosAngle);
            }
        }
        private void G�rerPicking()
        {
            if (GestionInput.EstNouveauClicDroit())
            {
                Point positionSouris = GestionInput.GetPositionSouris();
                if (Intersection(positionSouris, Roche))
                {

                }
            }
        }
        private float? DistanceIntersection(BoundingSphere sphereDeCollision, Point positionSouris)
        {
            Ray ray = TrouverPositionSourisPicking(positionSouris);
            return ray.Intersects(sphereDeCollision);
        }
        private bool Intersection(Point positionSouris, Model model)
        {
            for (int i = 0; i < model.Meshes.Count; i++)
            {
                BoundingSphere sphereDeCollision = model.Meshes[i].BoundingSphere;
                sphereDeCollision = sphereDeCollision.Transform(Matrix.Identity);
                float? distance = DistanceIntersection(sphereDeCollision, positionSouris);
                if (distance != null)
                {
                    return true;
                }
            }

            return false;
        }
        protected override void CalculerMonde()
        {
            Monde = Matrix.Identity;
            Monde *= Matrix.CreateScale(�chelle);
            Monde *= Matrix.CreateFromAxisAngle(Vector3.Up, Angle);
            Monde *= Matrix.CreateTranslation(Position);
        }

        private void G�rerClavierMouvement()
        {
            if (GestionInput.EstEnfonc�e(Keys.A))
            {
                Position += new Vector3(-VitesseD�placement, 0, 0);
            }
            if (GestionInput.EstEnfonc�e(Keys.D))
            {
                Position += new Vector3(VitesseD�placement, 0, 0);
            }
            if (GestionInput.EstEnfonc�e(Keys.W))
            {
                Position += new Vector3(0, 0, -VitesseD�placement);
            }
            if (GestionInput.EstEnfonc�e(Keys.S))
            {
                Position += new Vector3(0, 0, VitesseD�placement);
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
