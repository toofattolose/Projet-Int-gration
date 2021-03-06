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
    public class Joueur : Model3DAvecCollision
    {
        Random GenerateurAleatoire { get; set; }
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
        Vector2 PositionMenuDansÉcran { get; set; }
        public bool EstEnCollectionRessource { get; set; }
        
        float TempsÉcouléDepuisDernierTir { get; set; }
        Vector3 Direction { get; set; }
        Model Roche { get; set; }
        const float DELTA = 256f / 64;

        public int NombreDeBois { get; set; }
        public int NombreDOR { get; set; }
        public int NombrePtsKill { get; set; }

        SoundEffect SoundShooting { get; set; }
        
        GridDeJeu Grid { get; set; }
        PlacementBuilding BuildingEnPlacement { get; set; }

        byte TypeBuildingSelectionner { get; set; }

        //Achat Enemy
        MenuAchatEnemy MenuEnemy { get; set; }
        EnemyIcon IconEnemy1 { get; set; }
        EnemyIcon IconEnemy2 { get; set; }
        EnemyIcon IconEnemy3 { get; set; }
        EnemyIcon IconEnemy4 { get; set; }
        EnemyIcon IconEnemy5 { get; set; }
        EnemyIcon IconEnemy6 { get; set; }
        EnemyIcon IconEnemy7 { get; set; }
        EnemyIcon IconEnemy8 { get; set; }
        EnemyIcon IconEnemy9 { get; set; }
        EnemyIcon IconEnemy10 { get; set; }
        EnemyIcon IconEnemy11 { get; set; }
        EnemyIcon IconEnemy12 { get; set; }

        //Upgrade
        MenuUpgradeJoueur MenuUpgrade { get; set; }
        UpgradeJoueurDommage IconUpgradeDommage { get; set; }
        UpgradeJoueurFiringRate IconUpgradeFiringRate { get; set; }
        UpgradeJoueurTempsRécolte IconUpgradeTempsRécolte { get; set; }
        UpgradeJoueurNombreRécolte IconUpgradeNombreRécolte { get; set; }
        UpgradeMur IconUpgradeMur { get; set; }
        UpgradeGeneratrice IconUpgradeGeneratrice { get; set; }
        UpgradeReparateur IconUpgradeReparateur { get; set; }
        UpgradeTurret IconUpgradeTurret { get; set; }

        public int NiveauDommage { get; set; }
        public int NiveauTempsRécolte { get; set; }
        public int NiveauNombreRécolte { get; set; }
        public int NiveauFiringRate { get; set; }

        public float TempsCollectionRessource { get; set; }
        public int NombreCollectionRessource { get; set; }
        public float FiringRate { get; set; }
        public int Dommage { get; set; }
        public int NbPtsDeVieMax { get; set; }
        public int NbPtsDeVie { get; set; }

        Mur MurSélecitonné { get; set; }
        Generatrice GeneratriceSélectionné { get; set; }
        Reparateur ReparateurSélectionné { get; set; }
        Turret TurretSélectionné { get; set; }

        //Enemy
        int[] PrixEnemy { get; set; }




        public Joueur(Game game, string nomModele, float échelle, Vector3 position, Vector3 rotationInitiale, float intervalleMAJ)
            : base(game, nomModele, échelle, position, rotationInitiale)
        {
            IntervalleMAJ = intervalleMAJ;
        }

        public override void Initialize()
        {
            //Upgrade
            PositionMenuDansÉcran = new Vector2((1280 - 800) / 2, 720-250);
            NiveauDommage = 1;
            NiveauFiringRate = 1;
            NiveauNombreRécolte = 1;
            NiveauTempsRécolte = 1;
            NbPtsDeVieMax = 1;
            NbPtsDeVie = NbPtsDeVieMax;
            EstEnCollectionRessource = false;

            SoundShooting = Game.Content.Load<SoundEffect>("SoundEffects/shooting");
            État = "enMouvement";
            Grid = Game.Services.GetService(typeof(GridDeJeu)) as GridDeJeu;
            VitesseDéplacement = 0.25f;
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GestionnaireDeModèles = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            Roche = GestionnaireDeModèles.Find("rock1");
            CaméraJeu = Game.Services.GetService(typeof(Caméra)) as Caméra3rdPerson;
            GestionSouris = Mouse.GetState(); //utilisé pour trouver la position de la souris
            BuildingEnPlacement = new PlacementBuilding(Game, "tree2", 0.02f, Position, Vector3.Zero);
            GenerateurAleatoire = Game.Services.GetService(typeof(Random)) as Random;

            //Initialisation des données de stats du joueur
            FiringRate = 1;
            Dommage = 1;
            NombreCollectionRessource = 1;
            TempsCollectionRessource = 1f;

            //Enemy
            PrixEnemy = new int[12];
            PrixEnemy[0] = 5;
            PrixEnemy[1] = 25;
            PrixEnemy[2] = 100;
            PrixEnemy[3] = 400;
            PrixEnemy[4] = 1600;
            PrixEnemy[5] = 6400;
            PrixEnemy[6] = 25600;
            PrixEnemy[7] = 102400;
            PrixEnemy[8] = 409600;
            PrixEnemy[9] = 1638400;
            PrixEnemy[10] = 6553600;
            PrixEnemy[11] = 26214400;
            int offsetX = 96;
            int offsetY = 16;
            int offsetInbetween = 128;
            IconEnemy1 = new EnemyIcon(Game, new Vector2(offsetX, offsetY), "Sprites/spr_enemy_icon_0", 1, PrixEnemy[0]);
            IconEnemy2 = new EnemyIcon(Game, new Vector2(offsetX + (2 * offsetInbetween), offsetY), "Sprites/spr_enemy_icon_0", 2, PrixEnemy[1]);
            IconEnemy3 = new EnemyIcon(Game, new Vector2(offsetX + (4 * offsetInbetween), offsetY), "Sprites/spr_enemy_icon_0", 3, PrixEnemy[2]);
            IconEnemy4 = new EnemyIcon(Game, new Vector2(offsetX, offsetY + offsetInbetween), "Sprites/spr_enemy_icon_0", 4, PrixEnemy[3]);
            IconEnemy5 = new EnemyIcon(Game, new Vector2(offsetX + (2 * offsetInbetween), offsetY + offsetInbetween), "Sprites/spr_enemy_icon_0", 5, PrixEnemy[4]);
            IconEnemy6 = new EnemyIcon(Game, new Vector2(offsetX + (4 * offsetInbetween), offsetY + offsetInbetween), "Sprites/spr_enemy_icon_0", 6, PrixEnemy[5]);
            IconEnemy7 = new EnemyIcon(Game, new Vector2(offsetX, offsetY + (2 * offsetInbetween)), "Sprites/spr_enemy_icon_0", 7, PrixEnemy[6]);
            IconEnemy8 = new EnemyIcon(Game, new Vector2(offsetX + (2 * offsetInbetween), offsetY + (2 * offsetInbetween)), "Sprites/spr_enemy_icon_0", 8, PrixEnemy[7]);
            IconEnemy9 = new EnemyIcon(Game, new Vector2(offsetX + (4 * offsetInbetween), offsetY + (2 * offsetInbetween)), "Sprites/spr_enemy_icon_0", 9, PrixEnemy[8]);
            IconEnemy10 = new EnemyIcon(Game, new Vector2(offsetX, offsetY + (3 * offsetInbetween)), "Sprites/spr_enemy_icon_0", 10, PrixEnemy[9]);
            IconEnemy11 = new EnemyIcon(Game, new Vector2(offsetX + (2 * offsetInbetween), offsetY + (3 * offsetInbetween)), "Sprites/spr_enemy_icon_0", 11, PrixEnemy[10]);
            IconEnemy12 = new EnemyIcon(Game, new Vector2(offsetX + (4 * offsetInbetween), offsetY + (3 * offsetInbetween)), "Sprites/spr_enemy_icon_0", 12, PrixEnemy[11]);
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
                case ("estEnAchatEnemy"):
                    EstEnAchatEnemy(gameTime);
                    break;
            }
            GérerVie();
            GestionPerformance();
        }

        private void GérerVie()
        {
            if (NbPtsDeVie <= 0)
            {
                État = "estMort";
            }
        }

        private void GestionPerformance()
        {
            try
            {
                foreach (Model3D m in Game.Components.OfType<Model3D>())
                {
                    int offsetX = 48;
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
            catch(Exception) { }
            //Enleve les modeles qui sont trop loins
            
        }

        private void EstEnAchatEnemy(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                TempsÉcouléDepuisMAJ = 0;
            }
            if (GestionInput.EstNouvelleTouche(Keys.B))
            {
                MenuEnemy.Dispose();
                IconEnemy1.Dispose();
                IconEnemy2.Dispose();
                IconEnemy3.Dispose();
                IconEnemy4.Dispose();
                IconEnemy5.Dispose();
                IconEnemy6.Dispose();
                IconEnemy7.Dispose();
                IconEnemy8.Dispose();
                IconEnemy9.Dispose();
                IconEnemy10.Dispose();
                IconEnemy11.Dispose();
                IconEnemy12.Dispose();
                Game.Components.Add(BuildingEnPlacement);
                État = "enConstruction";
            }
            if (GestionInput.EstNouvelleTouche(Keys.M))
            {
                MenuEnemy.Dispose();
                IconEnemy1.Dispose();
                IconEnemy2.Dispose();
                IconEnemy3.Dispose();
                IconEnemy4.Dispose();
                IconEnemy5.Dispose();
                IconEnemy6.Dispose();
                IconEnemy7.Dispose();
                IconEnemy8.Dispose();
                IconEnemy9.Dispose();
                IconEnemy10.Dispose();
                IconEnemy11.Dispose();
                IconEnemy12.Dispose();
                MenuUpgrade = new MenuUpgradeJoueur(Game, PositionMenuDansÉcran);
                IconUpgradeDommage = new UpgradeJoueurDommage(Game, new Vector2(PositionMenuDansÉcran.X + 32, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon1");
                IconUpgradeFiringRate = new UpgradeJoueurFiringRate(Game, new Vector2(PositionMenuDansÉcran.X + 32 + 128, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon2");
                IconUpgradeTempsRécolte = new UpgradeJoueurTempsRécolte(Game, new Vector2(PositionMenuDansÉcran.X + 32 + 128 + 128, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon3");
                IconUpgradeNombreRécolte = new UpgradeJoueurNombreRécolte(Game, new Vector2(PositionMenuDansÉcran.X + 32 + 128 + 128 + 128, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon4");
                Game.Components.Add(MenuUpgrade);
                Game.Components.Add(IconUpgradeDommage);
                Game.Components.Add(IconUpgradeFiringRate);
                Game.Components.Add(IconUpgradeTempsRécolte);
                Game.Components.Add(IconUpgradeNombreRécolte);
                État = "estEnUpgrade";
            }
            if (GestionInput.EstNouvelleTouche(Keys.E))
            {
                MenuEnemy.Dispose();
                IconEnemy1.Dispose();
                IconEnemy2.Dispose();
                IconEnemy3.Dispose();
                IconEnemy4.Dispose();
                IconEnemy5.Dispose();
                IconEnemy6.Dispose();
                IconEnemy7.Dispose();
                IconEnemy8.Dispose();
                IconEnemy9.Dispose();
                IconEnemy10.Dispose();
                IconEnemy11.Dispose();
                IconEnemy12.Dispose();
                État = "enMouvement";
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
                if (TypeBuildingSelectionner == (byte)TypeUpgrade.Mur)
                {
                    IconUpgradeMur.Dispose();
                }
                else
                {
                    if (TypeBuildingSelectionner == (byte)TypeUpgrade.Generatrice)
                    {
                        IconUpgradeGeneratrice.Dispose();
                    }
                    else
                    {
                        if (TypeBuildingSelectionner == (byte)TypeUpgrade.Reparateur)
                        {
                            IconUpgradeReparateur.Dispose();
                        }
                        else
                        {
                            if (TypeBuildingSelectionner == (byte)TypeUpgrade.Turret)
                            {
                                IconUpgradeTurret.Dispose();
                            }
                        }
                    }
                }
                Game.Components.Add(BuildingEnPlacement);
                État = "enConstruction";
            }
            if (GestionInput.EstNouvelleTouche(Keys.M))
            {
                MenuUpgrade.Dispose();
                if (TypeBuildingSelectionner == (byte)TypeUpgrade.Mur)
                {
                    IconUpgradeMur.Dispose();
                }
                else
                {
                    if (TypeBuildingSelectionner == (byte)TypeUpgrade.Generatrice)
                    {
                        IconUpgradeGeneratrice.Dispose();
                    }
                    else
                    {
                        if (TypeBuildingSelectionner == (byte)TypeUpgrade.Reparateur)
                        {
                            IconUpgradeReparateur.Dispose();
                        }
                        else
                        {
                            if (TypeBuildingSelectionner == (byte)TypeUpgrade.Turret)
                            {
                                IconUpgradeTurret.Dispose();
                            }
                        }
                    }
                }
                MenuUpgrade = new MenuUpgradeJoueur(Game, PositionMenuDansÉcran);
                IconUpgradeDommage = new UpgradeJoueurDommage(Game, new Vector2(PositionMenuDansÉcran.X + 32, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon1");
                IconUpgradeFiringRate = new UpgradeJoueurFiringRate(Game, new Vector2(PositionMenuDansÉcran.X + 32 + 128, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon2");
                IconUpgradeTempsRécolte = new UpgradeJoueurTempsRécolte(Game, new Vector2(PositionMenuDansÉcran.X + 32 + 128 + 128, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon3");
                IconUpgradeNombreRécolte = new UpgradeJoueurNombreRécolte(Game, new Vector2(PositionMenuDansÉcran.X + 32 + 128 + 128 + 128, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon4");
                Game.Components.Add(MenuUpgrade);
                Game.Components.Add(IconUpgradeDommage);
                Game.Components.Add(IconUpgradeFiringRate);
                Game.Components.Add(IconUpgradeTempsRécolte);
                Game.Components.Add(IconUpgradeNombreRécolte);
                État = "estEnUpgrade";
            }

            if (GestionInput.EstNouveauClicDroit())
            {
                MenuUpgrade.Dispose();
                if (TypeBuildingSelectionner == (byte)TypeUpgrade.Mur)
                {
                    IconUpgradeMur.Dispose();
                }
                else
                {
                    if (TypeBuildingSelectionner == (byte)TypeUpgrade.Generatrice)
                    {
                        IconUpgradeGeneratrice.Dispose();
                    }
                    else
                    {
                        if (TypeBuildingSelectionner == (byte)TypeUpgrade.Reparateur)
                        {
                            IconUpgradeReparateur.Dispose();
                        }
                        else
                        {
                            if (TypeBuildingSelectionner == (byte)TypeUpgrade.Turret)
                            {
                                IconUpgradeTurret.Dispose();
                            }
                        }
                    }
                }
                État = "enMouvement";
            }

            if (GestionInput.EstNouvelleTouche(Keys.E))
            {
                MenuUpgrade.Dispose();
                if (TypeBuildingSelectionner == (byte)TypeUpgrade.Mur)
                {
                    IconUpgradeMur.Dispose();
                }
                else
                {
                    if (TypeBuildingSelectionner == (byte)TypeUpgrade.Generatrice)
                    {
                        IconUpgradeGeneratrice.Dispose();
                    }
                    else
                    {
                        if (TypeBuildingSelectionner == (byte)TypeUpgrade.Reparateur)
                        {
                            IconUpgradeReparateur.Dispose();
                        }
                        else
                        {
                            if (TypeBuildingSelectionner == (byte)TypeUpgrade.Turret)
                            {
                                IconUpgradeTurret.Dispose();
                            }
                        }
                    }
                }
                MenuEnemy = new MenuAchatEnemy(Game);
                Game.Components.Add(MenuEnemy);
                Game.Components.Add(IconEnemy1);
                Game.Components.Add(IconEnemy2);
                Game.Components.Add(IconEnemy3);
                Game.Components.Add(IconEnemy4);
                Game.Components.Add(IconEnemy5);
                Game.Components.Add(IconEnemy6);
                Game.Components.Add(IconEnemy7);
                Game.Components.Add(IconEnemy8);
                Game.Components.Add(IconEnemy9);
                Game.Components.Add(IconEnemy10);
                Game.Components.Add(IconEnemy11);
                Game.Components.Add(IconEnemy12);
                État = "estEnAchatEnemy";
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
            if (GestionInput.EstNouvelleTouche(Keys.E))
            {
                MenuUpgrade.Dispose();
                IconUpgradeDommage.Dispose();
                IconUpgradeFiringRate.Dispose();
                IconUpgradeTempsRécolte.Dispose();
                IconUpgradeNombreRécolte.Dispose();

                MenuEnemy = new MenuAchatEnemy(Game);
                Game.Components.Add(MenuEnemy);
                Game.Components.Add(IconEnemy1);
                Game.Components.Add(IconEnemy2);
                Game.Components.Add(IconEnemy3);
                Game.Components.Add(IconEnemy4);
                Game.Components.Add(IconEnemy5);
                Game.Components.Add(IconEnemy6);
                Game.Components.Add(IconEnemy7);
                Game.Components.Add(IconEnemy8);
                Game.Components.Add(IconEnemy9);
                Game.Components.Add(IconEnemy10);
                Game.Components.Add(IconEnemy11);
                Game.Components.Add(IconEnemy12);
                État = "estEnAchatEnemy";
            }
        }

        //mise a jour pour les mouvements du jouer
        private void FaireMAJMouvement(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            TempsSpawn += tempsÉcoulé;
            //GérerTir(gameTime);
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
                    Game.Components.Add(BuildingEnPlacement);
                    État = "enConstruction";
                }
                if (GestionInput.EstNouvelleTouche(Keys.M))
                {
                    MenuUpgrade = new MenuUpgradeJoueur(Game, PositionMenuDansÉcran);
                IconUpgradeDommage = new UpgradeJoueurDommage(Game, new Vector2(PositionMenuDansÉcran.X + 32, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon1");
                IconUpgradeFiringRate = new UpgradeJoueurFiringRate(Game, new Vector2(PositionMenuDansÉcran.X + 32 + 128, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon2");
                IconUpgradeTempsRécolte = new UpgradeJoueurTempsRécolte(Game, new Vector2(PositionMenuDansÉcran.X + 32 + 128 + 128, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon3");
                IconUpgradeNombreRécolte = new UpgradeJoueurNombreRécolte(Game, new Vector2(PositionMenuDansÉcran.X + 32 + 128 + 128 + 128, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon4");
                Game.Components.Add(MenuUpgrade);
                Game.Components.Add(IconUpgradeDommage);
                Game.Components.Add(IconUpgradeFiringRate);
                Game.Components.Add(IconUpgradeTempsRécolte);
                Game.Components.Add(IconUpgradeNombreRécolte);
                    État = "estEnUpgrade";
                }
            if (GestionInput.EstNouvelleTouche(Keys.E))
            {
                MenuEnemy = new MenuAchatEnemy(Game);
                Game.Components.Add(MenuEnemy);
                Game.Components.Add(IconEnemy1);
                Game.Components.Add(IconEnemy2);
                Game.Components.Add(IconEnemy3);
                Game.Components.Add(IconEnemy4);
                Game.Components.Add(IconEnemy5);
                Game.Components.Add(IconEnemy6);
                Game.Components.Add(IconEnemy7);
                Game.Components.Add(IconEnemy8);
                Game.Components.Add(IconEnemy9);
                Game.Components.Add(IconEnemy10);
                Game.Components.Add(IconEnemy11);
                Game.Components.Add(IconEnemy12);
                État = "estEnAchatEnemy";
            }
        }
        private void GérerTir(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsÉcouléDepuisDernierTir == 0)
            {
                if (GestionInput.EstAncienClicGauche())
                {
                    SoundShooting.Play();
                    Game.Components.Add(new BalleJoueur(Game, "bullet", 0.015f, Position + new Vector3(0, 2.5f, 0), Rotation, Dommage, Direction,1/60f, 2f));
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
                    //destruction de plantes
                    foreach (Plante p in Game.Components.OfType<Plante>())
                    {
                        for (int i = 0; i < p.Modèle.Meshes.Count; i++)
                        {
                            float distanceJoueur = (float)Math.Sqrt(Math.Pow(p.Position.X - Position.X, 2) + Math.Pow(p.Position.Y - Position.Y, 2) + Math.Pow(p.Position.Z - Position.Z, 2));
                            if (distanceJoueur <= 8)
                            {
                                if (TrouverIntersection(positionSouris, p.Position))
                                {
                                    p.EstCliquéDroit();
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
                                if (TrouverIntersection(positionSouris, m.Position) && État == "enMouvement")
                                {
                                    MenuUpgrade = new MenuUpgradeJoueur(Game, PositionMenuDansÉcran);
                                    IconUpgradeMur = new UpgradeMur(Game, new Vector2(PositionMenuDansÉcran.X + 32, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon1", m);
                                    Game.Components.Add(MenuUpgrade);
                                    Game.Components.Add(IconUpgradeMur);
                                    MurSélecitonné = m;
                                    TypeBuildingSelectionner = (byte)TypeUpgrade.Mur;
                                    État = "enUpgradeBuilding";
                                }
                            }
                        }
                    }

                    //sélection de la génératrice
                    foreach (Generatrice g in Game.Components.OfType<Generatrice>())
                    {
                        for (int i = 0; i < g.Modèle.Meshes.Count; i++)
                        {
                            float distanceJoueur = (float)Math.Sqrt(Math.Pow(g.Position.X - Position.X, 2) + Math.Pow(g.Position.Y - Position.Y, 2) + Math.Pow(g.Position.Z - Position.Z, 2));
                            if (distanceJoueur <= 40)
                            {
                                if (TrouverIntersection(positionSouris, g.Position) && État == "enMouvement")
                                {
                                    MenuUpgrade = new MenuUpgradeJoueur(Game,PositionMenuDansÉcran);
                                    IconUpgradeGeneratrice = new UpgradeGeneratrice(Game, new Vector2(PositionMenuDansÉcran.X + 32, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon1", g);
                                    Game.Components.Add(MenuUpgrade);
                                    Game.Components.Add(IconUpgradeGeneratrice);
                                    GeneratriceSélectionné = g;
                                    TypeBuildingSelectionner = (byte)TypeUpgrade.Generatrice;
                                    État = "enUpgradeBuilding";
                                }
                            }
                        }
                    }

                    //sélection du reparateur
                    foreach (Reparateur r in Game.Components.OfType<Reparateur>())
                    {
                        for (int i = 0; i < r.Modèle.Meshes.Count; i++)
                        {
                            float distanceJoueur = (float)Math.Sqrt(Math.Pow(r.Position.X - Position.X, 2) + Math.Pow(r.Position.Y - Position.Y, 2) + Math.Pow(r.Position.Z - Position.Z, 2));
                            if (distanceJoueur <= 40)
                            {
                                if (TrouverIntersection(positionSouris, r.Position) && État == "enMouvement")
                                {
                                    MenuUpgrade = new MenuUpgradeJoueur(Game, PositionMenuDansÉcran);
                                    IconUpgradeReparateur = new UpgradeReparateur(Game, new Vector2(PositionMenuDansÉcran.X + 32, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon1", r);
                                    Game.Components.Add(MenuUpgrade);
                                    Game.Components.Add(IconUpgradeReparateur);
                                    ReparateurSélectionné = r;
                                    TypeBuildingSelectionner = (byte)TypeUpgrade.Reparateur;
                                    État = "enUpgradeBuilding";
                                }
                            }
                        }
                    }

                    //sélection de la turret
                    foreach (Turret t in Game.Components.OfType<Turret>())
                    {
                        for (int i = 0; i < t.Modèle.Meshes.Count; i++)
                        {
                            float distanceJoueur = (float)Math.Sqrt(Math.Pow(t.Position.X - Position.X, 2) + Math.Pow(t.Position.Y - Position.Y, 2) + Math.Pow(t.Position.Z - Position.Z, 2));
                            if (distanceJoueur <= 40)
                            {
                                if (TrouverIntersection(positionSouris, t.Position) && État == "enMouvement")
                                {
                                    MenuUpgrade = new MenuUpgradeJoueur(Game, PositionMenuDansÉcran);
                                    IconUpgradeTurret = new UpgradeTurret(Game, new Vector2(PositionMenuDansÉcran.X + 32, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon1", t);
                                    Game.Components.Add(MenuUpgrade);
                                    Game.Components.Add(IconUpgradeTurret);
                                    TurretSélectionné = t;
                                    TypeBuildingSelectionner = (byte)TypeUpgrade.Turret;
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
                    Vector3 positionPossibleRessource = new Vector3((int)positionRessource.X - (int)(DELTA / 2) + i, 0, (int)positionRessource.Z - (int)(DELTA / 2) + j);
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
                    if (TypeBuildingSelectionner == (byte)TypeUpgrade.Mur)
                    {
                        IconUpgradeMur.Dispose();
                    }
                    else
                    {
                        if (TypeBuildingSelectionner == (byte)TypeUpgrade.Generatrice)
                        {
                            IconUpgradeGeneratrice.Dispose();
                        }
                        else
                        {
                            if (TypeBuildingSelectionner == (byte)TypeUpgrade.Reparateur)
                            {
                                IconUpgradeReparateur.Dispose();
                            }
                            else
                            {
                                if (TypeBuildingSelectionner == (byte)TypeUpgrade.Turret)
                                {
                                    IconUpgradeTurret.Dispose();
                                }
                            }
                        }
                    }
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
                    if (TypeBuildingSelectionner == (byte)TypeUpgrade.Mur)
                    {
                        IconUpgradeMur.Dispose();
                    }
                    else
                    {
                        if (TypeBuildingSelectionner == (byte)TypeUpgrade.Generatrice)
                        {
                            IconUpgradeGeneratrice.Dispose();
                        }
                        else
                        {
                            if (TypeBuildingSelectionner == (byte)TypeUpgrade.Reparateur)
                            {
                                IconUpgradeReparateur.Dispose();
                            }
                            else
                            {
                                if (TypeBuildingSelectionner == (byte)TypeUpgrade.Turret)
                                {
                                    IconUpgradeTurret.Dispose();
                                }
                            }
                    }
                    }
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
                    if (TypeBuildingSelectionner == (byte)TypeUpgrade.Mur)
                    {
                        IconUpgradeMur.Dispose();
                    }
                    else
                    {
                        if (TypeBuildingSelectionner == (byte)TypeUpgrade.Generatrice)
                        {
                            IconUpgradeGeneratrice.Dispose();
                        }
                        else
                        {
                            if (TypeBuildingSelectionner == (byte)TypeUpgrade.Reparateur)
                            {
                                IconUpgradeReparateur.Dispose();
                            }
                            else
                            {
                                if (TypeBuildingSelectionner == (byte)TypeUpgrade.Turret)
                                {
                                    IconUpgradeTurret.Dispose();
                                }
                            }
                        }
                    }
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
                    if (TypeBuildingSelectionner == (byte)TypeUpgrade.Mur)
                    {
                        IconUpgradeMur.Dispose();
                    }
                    else
                    {
                        if (TypeBuildingSelectionner == (byte)TypeUpgrade.Generatrice)
                        {
                            IconUpgradeGeneratrice.Dispose();
                        }
                        else
                        {
                            if (TypeBuildingSelectionner == (byte)TypeUpgrade.Reparateur)
                            {
                                IconUpgradeReparateur.Dispose();
                            }
                            else
                            {
                                if (TypeBuildingSelectionner == (byte)TypeUpgrade.Turret)
                                {
                                    IconUpgradeTurret.Dispose();
                                }
                            }
                    }           
                    }
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
                GérerRotationJoueur();
                TempsÉcouléDepuisMAJ = 0;
            }
                if (GestionInput.EstNouvelleTouche(Keys.M))
                {
                foreach (CaseDeConstruction c in Game.Components.OfType<CaseDeConstruction>())
                {
                    if (c.Visible)
                    {
                        c.Visible = false;
                    }
                }
                BuildingEnPlacement.Dispose();
                    MenuUpgrade = new MenuUpgradeJoueur(Game, PositionMenuDansÉcran);
                IconUpgradeDommage = new UpgradeJoueurDommage(Game, new Vector2(PositionMenuDansÉcran.X + 32, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon1");
                IconUpgradeFiringRate = new UpgradeJoueurFiringRate(Game, new Vector2(PositionMenuDansÉcran.X + 32 + 128, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon2");
                IconUpgradeTempsRécolte = new UpgradeJoueurTempsRécolte(Game, new Vector2(PositionMenuDansÉcran.X + 32 + 128 + 128, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon3");
                IconUpgradeNombreRécolte = new UpgradeJoueurNombreRécolte(Game, new Vector2(PositionMenuDansÉcran.X + 32 + 128 + 128 + 128, PositionMenuDansÉcran.Y + 36 + 50), "Sprites/spr_upgrade_icon4");
                Game.Components.Add(MenuUpgrade);
                Game.Components.Add(IconUpgradeDommage);
                Game.Components.Add(IconUpgradeFiringRate);
                Game.Components.Add(IconUpgradeTempsRécolte);
                Game.Components.Add(IconUpgradeNombreRécolte);
                    État = "estEnUpgrade";
                }
            if (GestionInput.EstNouvelleTouche(Keys.E))
            {
                foreach (CaseDeConstruction c in Game.Components.OfType<CaseDeConstruction>())
                {
                    if (c.Visible)
                    {
                        c.Visible = false;
                    }
                }
                BuildingEnPlacement.Dispose();
                MenuEnemy = new MenuAchatEnemy(Game);               
                Game.Components.Add(MenuEnemy);
                Game.Components.Add(IconEnemy1);
                Game.Components.Add(IconEnemy2);
                Game.Components.Add(IconEnemy3);
                Game.Components.Add(IconEnemy4);
                Game.Components.Add(IconEnemy5);
                Game.Components.Add(IconEnemy6);
                Game.Components.Add(IconEnemy7);
                Game.Components.Add(IconEnemy8);
                Game.Components.Add(IconEnemy9);
                Game.Components.Add(IconEnemy10);
                Game.Components.Add(IconEnemy11);
                Game.Components.Add(IconEnemy12);
                État = "estEnAchatEnemy";
            }
            GérerClavierConstruction();
        }

        private void GérerPickingConstruction()
        {
            Vector3 positionSouris = TrouverPositionSouris(GestionInput.GetPositionSouris());
            Vector2 positionSourisDansGrid = new Vector2((int)Math.Floor(positionSouris.X / Grid.Delta.X), (int)Math.Floor(positionSouris.Z / Grid.Delta.Y));
            
            foreach (CaseDeConstruction c in Game.Components.OfType<CaseDeConstruction>())
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
            int nombreParticuleSang = GenerateurAleatoire.Next(50, 100);
            for (int i = 0; i < nombreParticuleSang; i++)
            {
                Vector3 direction = new Vector3(GenerateurAleatoire.Next(-100, 100), GenerateurAleatoire.Next(0, 100), GenerateurAleatoire.Next(-100, 100));
                direction = Vector3.Normalize(direction);
                Sang particuleSang = new Sang(Game, "blood", 0.01f, new Vector3(Position.X,Position.Y + 5,Position.Z), new Vector3(GenerateurAleatoire.Next(0, 360), GenerateurAleatoire.Next(0, 360), GenerateurAleatoire.Next(0, 360)), direction);
                Game.Components.Add(particuleSang);
            }
            Dispose();
        }
        
        enum TypeUpgrade
        {
            Mur,
            Generatrice,
            Turret,
            Reparateur
        }
    }
}
