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
        RessourcesManager<Model> GestionnaireDeModèles { get; set; }
        float FiringRate { get; set; }
        int Dommage { get; set; }
        float TempsÉcouléDepuisDernierTir { get; set; }
        Vector3 Direction { get; set; }
        Model Roche { get; set; }
        const float DELTA = 256f / 64;

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
            GestionnaireDeModèles = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            Roche = GestionnaireDeModèles.Find("rock1");
            CaméraJeu = Game.Services.GetService(typeof(Caméra)) as Caméra3rdPerson;
            GestionSouris = Mouse.GetState(); //utilisé pour trouver la position de la souris

            //Initialisation des données de stats du joueur
            FiringRate = 0.5f;
            Dommage = 1;

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
            GérerTir(gameTime);
            GérerPicking();
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                GérerRotationJoueur();
                CaméraJeu.Déplacer(Position);
                CalculerMonde();
                TempsÉcouléDepuisMAJ = 0;
            }
        }

        private void GérerTir(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsÉcouléDepuisDernierTir == 0)
            {
                if (GestionInput.EstAncienClicGauche())
                {
                    Game.Components.Add(new BalleJoueur(Game, "bullet", 0.01f, Position + new Vector3(0,2.5f,0), Rotation, Dommage, Direction, 1 / 60f));
                    TempsÉcouléDepuisDernierTir += tempsÉcoulé;
                }
            }
            else
            {
                TempsÉcouléDepuisDernierTir += tempsÉcoulé;
                if (TempsÉcouléDepuisDernierTir >= FiringRate)
                {
                    TempsÉcouléDepuisDernierTir = 0;
                }
            }
        }

        //Utilisé pour trouver la position de la souris dans un environnement 3D
        private Vector3 TrouverPositionSouris(Point ms)
        {
            Vector2 positionSouris = new Vector2(ms.X, ms.Y);
            Vector3 nearScreenPoint = new Vector3(positionSouris, 0);
            Vector3 farScreenPoint = new Vector3(positionSouris, 1.01f);
            Vector3 nearWorldPoint = Game.GraphicsDevice.Viewport.Unproject(nearScreenPoint, CaméraJeu.Projection, CaméraJeu.Vue, Matrix.Identity);
            Vector3 farWorldPoint = Game.GraphicsDevice.Viewport.Unproject(farScreenPoint, CaméraJeu.Projection, CaméraJeu.Vue, Matrix.Identity);

            Vector3 direction = farWorldPoint - nearWorldPoint;

            float zFactor = -nearWorldPoint.Y / direction.Y;
            Vector3 zeroWorldPoint = nearWorldPoint + direction * zFactor;

            return zeroWorldPoint;
        }
        private void GérerRotationJoueur()
        {
            Point positionSourisInitiale = GestionInput.GetPositionSouris();
            Vector3 positionSouris = TrouverPositionSouris(positionSourisInitiale);
            Vector3 direction = new Vector3(positionSouris.X - Position.X, 0, positionSouris.Z - Position.Z);
            Vector3 directionBase = Vector3.UnitX;
            direction.Normalize();
            Direction = direction;
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
            Rotation = new Vector3(0, Angle, 0);
        }

        //Picking
        private void GérerPicking()
        {
            if (GestionInput.EstNouveauClicDroit())
            {
                Point positionSouris = GestionInput.GetPositionSouris();
                try
                {
                    foreach (Roche r in Game.Components.OfType<Roche>())
                    {
                        for (int i = 0; i < r.Modèle.Meshes.Count; i++)
                        {
                            float distanceJoueur = (float)Math.Sqrt(Math.Pow(r.Position.X - Position.X, 2) + Math.Pow(r.Position.Y - Position.Y, 2) + Math.Pow(r.Position.Z - Position.Z, 2));
                            if (distanceJoueur <= 5)
                            {
                                if (TrouverIntersection(positionSouris, r.Position, r.Modèle.Meshes[i].BoundingSphere))
                                {
                                    r.EstCliquéDroit();
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }    
            }
        }

        //Trouve l'intersection entre la position de la souris et la position de la roche
        private bool TrouverIntersection(Point positionSouris, Vector3 positionRessource, BoundingSphere sphere)
        {
            Vector3 posSouris3D = TrouverPositionSouris(positionSouris);
            Vector3 nouvellePositionSouris = new Vector3((int)posSouris3D.X, (int)posSouris3D.Y, (int)posSouris3D.Z);
            for (int i = 0; i < (int)DELTA * 2; i++)
            {
                for (int j = 0; j < (int)DELTA * 2; j++)
                {
                    Vector3 positionPossibleRessource = new Vector3((int)positionRessource.X + i, 0, (int)positionRessource.Z + j);
                    if (nouvellePositionSouris == positionPossibleRessource)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        protected override void CalculerMonde()
        {
            Monde = Matrix.Identity;
            Monde *= Matrix.CreateScale(Échelle);
            Monde *= Matrix.CreateFromAxisAngle(Vector3.Up, Angle);
            Monde *= Matrix.CreateTranslation(Position);
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
