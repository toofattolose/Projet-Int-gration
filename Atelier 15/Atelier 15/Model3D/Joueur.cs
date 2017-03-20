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
        public string État { get; set; }
        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        float VitesseDéplacement { get; set; }
        float Angle { get; set; }
        InputManager GestionInput { get; set; }
        Caméra CaméraJeu { get; set; }
        MouseState GestionSouris { get; set; }
        RessourcesManager<Model> GestionnaireDeModèles { get; set; }
        float TempsSpawn { get; set; }
        
        float TempsÉcouléDepuisDernierTir { get; set; }
        Vector3 Direction { get; set; }
        Model Roche { get; set; }
        const float DELTA = 256f / 64;

        public int NombreDeBois { get; set; }
        public int NombreDOR { get; set; }
        
        GridDeJeu Grid { get; set; }
        PlacementBuilding BuildingEnPlacement { get; set; }


        //Upgrade
        MenuUpgradeJoueur MenuUpgrade { get; set; }
        UpgradeJoueurDommage IconUpgradeDommage { get; set; }
        UpgradeJoueurFiringRate IconUpgradeFiringRate { get; set; }
        UpgradeJoueurTempsRécolte IconUpgradeTempsRécolte { get; set; }
        UpgradeJoueurNombreRécolte IconUpgradeNombreRécolte { get; set; }
        UpgradeMur IconUpgradeMur { get; set; }

        public int NiveauDommage { get; set; }
        public int NiveauTempsRécolte{ get; set; }
        public int NiveauNombreRécolte { get; set; }
        public int NiveauFiringRate { get; set; }

        public float TempsCollectionRessource { get; set; }
        public int NombreCollectionRessource { get; set; }
        public float FiringRate { get; set; }
        public int Dommage { get; set; }
        public int Vie { get; set; }
        float TempsSpawn { get; set; }

        Mur MurSélecitonné { get; set; }




        public Joueur(Game game, string nomModele, float échelle, Vector3 position, Vector3 rotationInitiale, float intervalleMAJ)
            : base(game, nomModele, échelle, position, rotationInitiale)
        {
            IntervalleMAJ = intervalleMAJ;
        }

        public override void Initialize()
        {
            //Upgrade
            NiveauDommage = 1;
            NiveauFiringRate = 1;
            NiveauNombreRécolte = 1;
            NiveauTempsRécolte = 1;
            Vie = 1;

            État = "enMouvement";
            Grid = Game.Services.GetService(typeof(GridDeJeu)) as GridDeJeu;
            VitesseDéplacement = 0.25f;
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GestionnaireDeModèles = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            Roche = GestionnaireDeModèles.Find("rock1");
            CaméraJeu = Game.Services.GetService(typeof(Caméra)) as Caméra3rdPerson;
            GestionSouris = Mouse.GetState(); //utilisé pour trouver la position de la souris
            BuildingEnPlacement = new PlacementBuilding(Game, "tree2", 0.02f, Position, Vector3.Zero);

            //Initialisation des données de stats du joueur
            FiringRate = 1;
            Dommage = 1;
            NombreCollectionRessource = 1;
            TempsCollectionRessource = 1f;

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
                case ("estEnUpgrade"):
                    EstEnUpgrade(gameTime);
                    break;
                case ("enUpgradeBuilding"):
                    EstEnUpgradeBuilding(gameTime);
                    break;
            }
            foreach (Model3D m in Game.Components.OfType<Model3D>())
            {
                int offsetX = 45;
                int offsetY = 20;
                if (m.Position.X < (Position.X - offsetX) || (m.Position.X > Position.X + offsetX) || (m.Position.Y < Position.Y - offsetY) || (m.Position.Y > Position.Y + offsetY))
                {
                    m.Visible = false;
        }
                else
                {
                    m.Visible = true;
                }
            }
        }

        private void EstEnUpgradeBuilding(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            TempsSpawn += tempsÉcoulé;
            GérerPicking();
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                GérerClavierMouvement();
                GérerRotationJoueur();
                CaméraJeu.Déplacer(Position);
                CalculerMonde();
                TempsÉcouléDepuisMAJ = 0;
            }
            if (GestionInput.EstNouvelleTouche(Keys.B))
            {
                MenuUpgrade.Dispose();
                IconUpgradeMur.Dispose();
                Game.Components.Add(BuildingEnPlacement);
                État = "enConstruction";
            }
            if (GestionInput.EstNouvelleTouche(Keys.M))
            {
                MenuUpgrade.Dispose();
                IconUpgradeMur.Dispose();
                MenuUpgrade = new MenuUpgradeJoueur(Game);
                IconUpgradeDommage = new UpgradeJoueurDommage(Game, new Vector2(32, 300 + 36), "Sprites/spr_upgrade_icon1");
                IconUpgradeFiringRate = new UpgradeJoueurFiringRate(Game, new Vector2(32 + 128, 300 + 36), "Sprites/spr_upgrade_icon2");
                IconUpgradeTempsRécolte = new UpgradeJoueurTempsRécolte(Game, new Vector2(32 + 128 + 128, 300 + 36), "Sprites/spr_upgrade_icon3");
                IconUpgradeNombreRécolte = new UpgradeJoueurNombreRécolte(Game, new Vector2(32 + 128 + 128 + 128, 300 + 36), "Sprites/spr_upgrade_icon4");
                Game.Components.Add(MenuUpgrade);
                Game.Components.Add(IconUpgradeDommage);
                Game.Components.Add(IconUpgradeFiringRate);
                Game.Components.Add(IconUpgradeTempsRécolte);
                Game.Components.Add(IconUpgradeNombreRécolte);
                État = "estEnUpgrade";
            }
        }

        private void EstEnUpgrade(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                CaméraJeu.Déplacer(Position);
                CalculerMonde();
                
                
                TempsÉcouléDepuisMAJ = 0;
            }
                if (GestionInput.EstNouvelleTouche(Keys.B))
                {
                MenuUpgrade.Dispose();
                IconUpgradeDommage.Dispose();
                IconUpgradeFiringRate.Dispose();
                IconUpgradeTempsRécolte.Dispose();
                IconUpgradeNombreRécolte.Dispose();
                    Game.Components.Add(BuildingEnPlacement);
                    État = "enConstruction";
                }
                if (GestionInput.EstNouvelleTouche(Keys.M))
                {
                    MenuUpgrade.Dispose();
                IconUpgradeDommage.Dispose();
                IconUpgradeFiringRate.Dispose();
                IconUpgradeTempsRécolte.Dispose();
                IconUpgradeNombreRécolte.Dispose();
                    État = "enMouvement";
                }
        }

        //mise a jour pour les mouvements du jouer
        private void FaireMAJMouvement(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            TempsSpawn += tempsÉcoulé;
            GérerTir(gameTime);
            GérerPicking();
            if(TempsSpawn >= 5)
            {
                Game.Components.Add(new Ennemis(Game, "player", 0.01f, new Vector3(256 / 2f + 2, 0, 256 / 2f + 2), Vector3.Zero));
                TempsSpawn = 0;
            }
            if(Vie <= 0)
            {
                État = "estMort";
            }
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                GérerClavierMouvement();
                GérerRotationJoueur();
                CaméraJeu.Déplacer(Position);
                CalculerMonde();
                
                
                TempsÉcouléDepuisMAJ = 0;
            }
                if (GestionInput.EstNouvelleTouche(Keys.B))
                {
                    Game.Components.Add(BuildingEnPlacement);
                    État = "enConstruction";
                }
                if (GestionInput.EstNouvelleTouche(Keys.M))
                {
                    MenuUpgrade = new MenuUpgradeJoueur(Game);
                IconUpgradeDommage = new UpgradeJoueurDommage(Game, new Vector2(32, 350 + 36), "Sprites/spr_upgrade_icon1");
                IconUpgradeFiringRate = new UpgradeJoueurFiringRate(Game, new Vector2(32 + 128, 350 + 36), "Sprites/spr_upgrade_icon2");
                IconUpgradeTempsRécolte = new UpgradeJoueurTempsRécolte(Game, new Vector2(32 + 128 + 128, 350 + 36), "Sprites/spr_upgrade_icon3");
                IconUpgradeNombreRécolte = new UpgradeJoueurNombreRécolte(Game, new Vector2(32 + 128 + 128 + 128, 350 + 36), "Sprites/spr_upgrade_icon4");
                    Game.Components.Add(MenuUpgrade);
                Game.Components.Add(IconUpgradeDommage);
                Game.Components.Add(IconUpgradeFiringRate);
                Game.Components.Add(IconUpgradeTempsRécolte);
                Game.Components.Add(IconUpgradeNombreRécolte);
                    État = "estEnUpgrade";
                }
        }
        private void GérerTir(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsÉcouléDepuisDernierTir == 0)
            {
                if (GestionInput.EstAncienClicGauche())
                {
                    Game.Components.Add(new BalleJoueur(Game, "bullet", 0.015f, Position + new Vector3(0,2.5f,0), Rotation, Dommage, Direction, 1 / 60f));
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
                    //destruction de roche
                    foreach (Roche r in Game.Components.OfType<Roche>())
                    {
                        for (int i = 0; i < r.Modèle.Meshes.Count; i++)
                        {
                            float distanceJoueur = (float)Math.Sqrt(Math.Pow(r.Position.X - Position.X, 2) + Math.Pow(r.Position.Y - Position.Y, 2) + Math.Pow(r.Position.Z - Position.Z, 2));
                            if (distanceJoueur <= 8)
                            {
                                if (TrouverIntersection(positionSouris, r.Position))
                                {
                                    r.EstCliquéDroit();
                                }
                            }
                        }
                    }
                    //collection de bois
                    foreach (Arbre a in Game.Components.OfType<Arbre>())
                    {
                        for (int i = 0; i < a.Modèle.Meshes.Count; i++)
                        {
                            float distanceJoueur = (float)Math.Sqrt(Math.Pow(a.Position.X - Position.X, 2) + Math.Pow(a.Position.Y - Position.Y, 2) + Math.Pow(a.Position.Z - Position.Z, 2));
                            if (distanceJoueur <= 8)
                            {
                                if (TrouverIntersection(positionSouris, a.Position))
                                {
                                    a.EstCliquéDroit(this);
                                }
                            }
                        }
                    }
                    //collection de l'or
                    foreach (RessourceOr o in Game.Components.OfType<RessourceOr>())
                    {
                        for (int i = 0; i < o.Modèle.Meshes.Count; i++)
                        {
                            float distanceJoueur = (float)Math.Sqrt(Math.Pow(o.Position.X - Position.X, 2) + Math.Pow(o.Position.Y - Position.Y, 2) + Math.Pow(o.Position.Z - Position.Z, 2));
                            if (distanceJoueur <= 8)
                            {
                                if (TrouverIntersection(positionSouris, o.Position))
                                {
                                    o.EstCliquéDroit(this);
                                }
                            }
                        }
                    }

                    //sélection du mur
                    foreach (Mur m in Game.Components.OfType<Mur>())
                    {
                        for (int i = 0; i < m.Modèle.Meshes.Count; i++)
                        {
                            float distanceJoueur = (float)Math.Sqrt(Math.Pow(m.Position.X - Position.X, 2) + Math.Pow(m.Position.Y - Position.Y, 2) + Math.Pow(m.Position.Z - Position.Z, 2));
                            if (distanceJoueur <= 40)
                            {
                                if (TrouverIntersection(positionSouris, m.Position))
                                {
                                    MenuUpgrade = new MenuUpgradeJoueur(Game);
                                    IconUpgradeMur = new UpgradeMur(Game, new Vector2(32, 350 + 36), "Sprites/spr_upgrade_icon1", m);
                                    Game.Components.Add(MenuUpgrade);
                                    Game.Components.Add(IconUpgradeMur);
                                    MurSélecitonné = m;
                                    État = "enUpgradeBuilding";
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
            Monde *= Matrix.CreateScale(Échelle);
            Monde *= Matrix.CreateFromAxisAngle(Vector3.Up, Angle);
            Monde *= Matrix.CreateTranslation(Position);
        }
        private void GérerClavierMouvement()
        {
            Vector3 déplacement;
            Vector3 déplacementAugmenter;
            if (GestionInput.EstEnfoncée(Keys.A))
            {
                déplacement = new Vector3(-VitesseDéplacement, 0, 0);
                déplacementAugmenter = déplacement + new Vector3(-DELTA / 4, 0, 0);
                if (VérifierSiDéplacementPossible(déplacementAugmenter))
                {
                    Position += déplacement;
                }
                if (État == "enUpgradeBuilding")
                {
                    MenuUpgrade.Dispose();
                    IconUpgradeMur.Dispose();
                    État = "enMouvement";
                }
            }
            if (GestionInput.EstEnfoncée(Keys.D))
            {
                déplacement = new Vector3(VitesseDéplacement, 0, 0);
                déplacementAugmenter = déplacement + new Vector3(DELTA / 4, 0, 0);
                if (VérifierSiDéplacementPossible(déplacementAugmenter))
                {
                    Position += déplacement;
                }
                if (État == "enUpgradeBuilding")
                {
                    MenuUpgrade.Dispose();
                    IconUpgradeMur.Dispose();
                    État = "enMouvement";
                }
            }
            if (GestionInput.EstEnfoncée(Keys.W))
            {
                déplacement = new Vector3(0, 0, -VitesseDéplacement);
                déplacementAugmenter = déplacement + new Vector3(0, 0, -DELTA / 4);
                if (VérifierSiDéplacementPossible(déplacementAugmenter))
                {
                    Position += déplacement;
                }
                if (État == "enUpgradeBuilding")
                {
                    MenuUpgrade.Dispose();
                    IconUpgradeMur.Dispose();
                    État = "enMouvement";
                }
            }
            if (GestionInput.EstEnfoncée(Keys.S))
            {
                déplacement = new Vector3(0, 0, VitesseDéplacement);
                déplacementAugmenter = déplacement + new Vector3(0, 0, DELTA / 4);
                if (VérifierSiDéplacementPossible(déplacementAugmenter))
                {
                    Position += déplacement;
                }
                if (État == "enUpgradeBuilding")
                {
                    MenuUpgrade.Dispose();
                    IconUpgradeMur.Dispose();
                    État = "enMouvement";
                }
            }
            
        }

        private bool VérifierSiDéplacementPossible(Vector3 déplacement)
        {
            Vector3 nouvellePosition = Position + déplacement;
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
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            GérerPickingConstruction();
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                CaméraJeu.Déplacer(Position);
                CalculerMonde();
                
                TempsÉcouléDepuisMAJ = 0;
            }
                GérerClavierConstruction();
                if (GestionInput.EstNouvelleTouche(Keys.M))
                {
                    MenuUpgrade = new MenuUpgradeJoueur(Game);
                    Game.Components.Add(MenuUpgrade);
                    État = "estEnUpgrade";
                }
        }

        private void GérerPickingConstruction()
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
        private void GérerClavierConstruction()
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
                BuildingEnPlacement.Dispose();
                État = "enMouvement";
            }
        }

        //mise a jour pour la mort du joueur
        private void EstMort(GameTime gameTime)
        {

        }
    }
}
