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
        float FiringRate { get; set; }
        int Dommage { get; set; }
        float Temps�coul�DepuisDernierTir { get; set; }
        Vector3 Direction { get; set; }
        Model Roche { get; set; }
        const float DELTA = 256f / 64;
        public int NombreDeBois { get; set; }
        public int NombreDOR { get; set; }
        float TempsCollectionRessource { get; set; }
        int NombreCollectionRessource { get; set; }
        GridDeJeu Grid { get; set; }




        public Joueur(Game game, string nomModele, float �chelle, Vector3 position, Vector3 rotationInitiale, float intervalleMAJ)
            : base(game, nomModele, �chelle, position, rotationInitiale)
        {
            IntervalleMAJ = intervalleMAJ;
        }

        public override void Initialize()
        {
            �tat = "enMouvement";
            Grid = Game.Services.GetService(typeof(GridDeJeu)) as GridDeJeu;
            VitesseD�placement = 0.2f;
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GestionnaireDeMod�les = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            Roche = GestionnaireDeMod�les.Find("rock1");
            Cam�raJeu = Game.Services.GetService(typeof(Cam�ra)) as Cam�ra3rdPerson;
            GestionSouris = Mouse.GetState(); //utilis� pour trouver la position de la souris

            //Initialisation des donn�es de stats du joueur
            FiringRate = 0.5f;
            Dommage = 1;
            NombreCollectionRessource = 1;
            TempsCollectionRessource = 1f;

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
            G�rerTir(gameTime);
            G�rerPicking();
            if (Temps�coul�DepuisMAJ >= IntervalleMAJ)
            {
                G�rerRotationJoueur();
                Cam�raJeu.D�placer(Position);
                CalculerMonde();
                Temps�coul�DepuisMAJ = 0;
            }
        }
        private void G�rerTir(GameTime gameTime)
        {
            float temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Temps�coul�DepuisDernierTir == 0)
            {
                if (GestionInput.EstAncienClicGauche())
                {
                    Game.Components.Add(new BalleJoueur(Game, "bullet", 0.01f, Position + new Vector3(0,2.5f,0), Rotation, Dommage, Direction, 1 / 60f));
                    Temps�coul�DepuisDernierTir += temps�coul�;
                }
            }
            else
            {
                Temps�coul�DepuisDernierTir += temps�coul�;
                if (Temps�coul�DepuisDernierTir >= FiringRate)
                {
                    Temps�coul�DepuisDernierTir = 0;
                }
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
        private void G�rerRotationJoueur()
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
        private void G�rerPicking()
        {
            if (GestionInput.EstNouveauClicDroit())
            {
                Point positionSouris = GestionInput.GetPositionSouris();
                try
                {
                    //destruction de roche
                    foreach (Roche r in Game.Components.OfType<Roche>())
                    {
                        for (int i = 0; i < r.Mod�le.Meshes.Count; i++)
                        {
                            float distanceJoueur = (float)Math.Sqrt(Math.Pow(r.Position.X - Position.X, 2) + Math.Pow(r.Position.Y - Position.Y, 2) + Math.Pow(r.Position.Z - Position.Z, 2));
                            if (distanceJoueur <= 8)
                            {
                                if (TrouverIntersection(positionSouris, r.Position))
                                {
                                    r.EstCliqu�Droit();
                                }
                            }
                        }
                    }
                    //collection de bois
                    foreach (Arbre a in Game.Components.OfType<Arbre>())
                    {
                        for (int i = 0; i < a.Mod�le.Meshes.Count; i++)
                        {
                            float distanceJoueur = (float)Math.Sqrt(Math.Pow(a.Position.X - Position.X, 2) + Math.Pow(a.Position.Y - Position.Y, 2) + Math.Pow(a.Position.Z - Position.Z, 2));
                            if (distanceJoueur <= 8)
                            {
                                if (TrouverIntersection(positionSouris, a.Position))
                                {
                                    a.EstCliqu�Droit(TempsCollectionRessource, NombreCollectionRessource, this);
                                }
                            }
                        }
                    }
                    //collection de l'or
                    foreach (RessourceOr o in Game.Components.OfType<RessourceOr>())
                    {
                        for (int i = 0; i < o.Mod�le.Meshes.Count; i++)
                        {
                            float distanceJoueur = (float)Math.Sqrt(Math.Pow(o.Position.X - Position.X, 2) + Math.Pow(o.Position.Y - Position.Y, 2) + Math.Pow(o.Position.Z - Position.Z, 2));
                            if (distanceJoueur <= 8)
                            {
                                if (TrouverIntersection(positionSouris, o.Position))
                                {
                                    o.EstCliqu�Droit(TempsCollectionRessource, NombreCollectionRessource, this);
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
        private bool TrouverIntersection(Point positionSouris, Vector3 positionRessource)
        {
            Vector3 posSouris3D = TrouverPositionSouris(positionSouris);
            Vector3 nouvellePositionSouris = new Vector3((int)posSouris3D.X, (int)posSouris3D.Y, (int)posSouris3D.Z);
            for (int i = 0; i < (int)DELTA; i++)
            {
                for (int j = 0; j < (int)DELTA; j++)
                {
                    Vector3 positionPossibleRessource = new Vector3((int)positionRessource.X - (int)(DELTA/2) + i, 0, (int)positionRessource.Z - (int)(DELTA / 2) + j);
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
            Monde *= Matrix.CreateScale(�chelle);
            Monde *= Matrix.CreateFromAxisAngle(Vector3.Up, Angle);
            Monde *= Matrix.CreateTranslation(Position);
        }
        private void G�rerClavierMouvement()
        {
            Vector3 d�placement;
            Vector3 d�placementAugmenter;
            if (GestionInput.EstEnfonc�e(Keys.A))
            {
                d�placement = new Vector3(-VitesseD�placement, 0, 0);
                d�placementAugmenter = d�placement + new Vector3(-DELTA / 4, 0, 0);
                if (V�rifierSiD�placementPossible(d�placementAugmenter))
                {
                    Position += d�placement;
                }
            }
            if (GestionInput.EstEnfonc�e(Keys.D))
            {
                d�placement = new Vector3(VitesseD�placement, 0, 0);
                d�placementAugmenter = d�placement + new Vector3(DELTA / 4, 0, 0);
                if (V�rifierSiD�placementPossible(d�placementAugmenter))
                {
                    Position += d�placement;
                }
            }
            if (GestionInput.EstEnfonc�e(Keys.W))
            {
                d�placement = new Vector3(0, 0, -VitesseD�placement);
                d�placementAugmenter = d�placement + new Vector3(0, 0, -DELTA / 4);
                if (V�rifierSiD�placementPossible(d�placementAugmenter))
                {
                    Position += d�placement;
                }
            }
            if (GestionInput.EstEnfonc�e(Keys.S))
            {
                d�placement = new Vector3(0, 0, VitesseD�placement);
                d�placementAugmenter = d�placement + new Vector3(0, 0, DELTA / 4);
                if (V�rifierSiD�placementPossible(d�placementAugmenter))
                {
                    Position += d�placement;
                }
            }
            if (GestionInput.EstNouvelleTouche(Keys.B))
            {
                �tat = "enConstruction";
            }
        }

        private bool V�rifierSiD�placementPossible(Vector3 d�placement)
        {
            Vector3 nouvellePosition = Position + d�placement;
            Vector2 positionDansGrid = new Vector2((float)Math.Floor(nouvellePosition.X / Grid.Delta.X), (float)Math.Floor(nouvellePosition.Z / Grid.Delta.Y));
            if ((int)positionDansGrid.X > Grid.TableauGrid.GetLength(0) - 2)
            {
                return false;
            }
            else
            {
                if ((int)positionDansGrid.X < 0)
                {
                    return false;
                }
            }
            if ((int)positionDansGrid.Y > Grid.TableauGrid.GetLength(1) - 2)
            {
                return false;
            }
            else
            {
                if ((int)positionDansGrid.Y < 0)
                {
                    return false;
                }
            }

            return Grid.TableauGrid[(int)positionDansGrid.X, (int)positionDansGrid.Y];
        }



        //mise a jour pour la construction
        private void FaireMAJConstruction(GameTime gameTime)
        {
            float temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps�coul�DepuisMAJ += temps�coul�;
            G�rerPickingConstruction();
            if (Temps�coul�DepuisMAJ >= IntervalleMAJ)
            {
                G�rerClavierConstruction();
                Cam�raJeu.D�placer(Position);
                CalculerMonde();
                Temps�coul�DepuisMAJ = 0;
            }
        }
        private void G�rerPickingConstruction()
        {
            Vector3 positionSouris = TrouverPositionSouris(GestionInput.GetPositionSouris());
            Vector2 positionSourisDansGrid = new Vector2((int)Math.Floor(positionSouris.X / Grid.Delta.X), (int)Math.Floor(positionSouris.Z / Grid.Delta.Y));
            
            foreach(CaseDeConstruction c in Game.Components.OfType<CaseDeConstruction>())
            {
                if (positionSourisDansGrid == c.PositionDansGrid)
                {
                    c.Visible = true;
                }
                else
                {
                    c.Visible = false;
                }
            }

        }
        private void G�rerClavierConstruction()
        {
            if (GestionInput.EstNouvelleTouche(Keys.B))
            {
                foreach (CaseDeConstruction c in Game.Components.OfType<CaseDeConstruction>())
                {
                    if (c.Visible)
                    {
                        c.Visible = false;
                    }
                }
                �tat = "enMouvement";
            }
        }

        //mise a jour pour la mort du joueur
        private void EstMort(GameTime gameTime)
        {

        }
    }
}
