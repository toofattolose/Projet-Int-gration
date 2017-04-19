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
    public class Ennemis : Model3DAvecCollision
    {
        const int Delta = 4;
        public int Vie { get; set; }
        public int Dmg { get; set; }
        Vector3 Objectif { get; set; }
        float Temps…coulÈMAJ { get; set; }
        float Angle { get; set; }
        Vector2 PositionDÈpart { get; set; }
        public PathFindingAStar PathFinding { get; set; }
        public List<Case> Path { get; set; }
        Joueur Player { get; set; }
        Case CaseEnnemi { get; set; }
        Case CaseSuivante { get; set; }
        Case CasePlayer { get; set; }
        Case CaseCible { get; set; }
        int IndexEnnemi { get; set; }
        int IndexAncien { get; set; }
        float Distance { get; set; }
        bool DÈplacer { get; set; }
        Vector3 DÈplacement { get; set; }
        int CompteurDÈplacement { get; set; }
        SoundEffect SoundDeath { get; set; }
        Vector3 Direction { get; set; }
        byte …tat { get; set; }
        GridDeJeu Grid { get; set; }
        Batiment BatimentCible { get; set; }
        Random GenerateurAleatoire { get; set; }

        float IntervalleAttaque { get; set; }

        //Enemy stats
        int Niveau { get; set; }
        int[,] StatsEnemy = new int[12, 3];

        public Ennemis(Game game, string nomModele, float Èchelle, Vector3 position, Vector3 rotationInitiale, int niveau)
            : base(game, nomModele, Èchelle, position, rotationInitiale)
        {
            PathFinding = new PathFindingAStar(game);
            Niveau = niveau;
            DÈplacer = true;
            StatsEnemy[0, 0] = 1; //Niveau
            StatsEnemy[0, 1] = 5; //Vie
            StatsEnemy[0, 2] = 1; //Dommage
            StatsEnemy[1, 0] = 2;
            StatsEnemy[1, 1] = 10;
            StatsEnemy[1, 2] = 1; //Dommage
            StatsEnemy[2, 0] = 3;
            StatsEnemy[2, 1] = 15;
            StatsEnemy[2, 2] = 1; //Dommage
            StatsEnemy[3, 0] = 4;
            StatsEnemy[3, 1] = 20;
            StatsEnemy[3, 2] = 1; //Dommage
            StatsEnemy[4, 0] = 5;
            StatsEnemy[4, 1] = 25;
            StatsEnemy[4, 2] = 1; //Dommage
            StatsEnemy[5, 0] = 6;
            StatsEnemy[5, 1] = 50;
            StatsEnemy[5, 2] = 1; //Dommage
            StatsEnemy[6, 0] = 7;
            StatsEnemy[6, 1] = 75;
            StatsEnemy[6, 2] = 1; //Dommage
            StatsEnemy[7, 0] = 8;
            StatsEnemy[7, 1] = 100;
            StatsEnemy[7, 2] = 1; //Dommage
            StatsEnemy[8, 0] = 9;
            StatsEnemy[8, 1] = 150;
            StatsEnemy[8, 2] = 1; //Dommage
            StatsEnemy[9, 0] = 10;
            StatsEnemy[9, 1] = 200;
            StatsEnemy[9, 2] = 1; //Dommage
            StatsEnemy[10, 0] = 11;
            StatsEnemy[10, 1] = 250;
            StatsEnemy[10, 2] = 1; //Dommage
            StatsEnemy[11, 0] = 12;
            StatsEnemy[11, 1] = 500;
            StatsEnemy[11, 2] = 1; //Dommage
        }

        public override void Initialize()
        {
            IntervalleAttaque = 2f;
            GenerateurAleatoire = Game.Services.GetService(typeof(Random)) as Random;
            Grid = Game.Services.GetService(typeof(GridDeJeu)) as GridDeJeu;
            SoundDeath = Game.Content.Load<SoundEffect>("SoundEffects/enemydeath");
            …tat = (byte)…tatEnnemi.RECHERCHE;
            try
            {
                foreach (Joueur j in Game.Components.OfType<Joueur>())
                {
                    Player = j;
                }
            }
            catch(Exception) { }
            for (int i = 0; i < StatsEnemy.GetLength(0); i++)
            {
                if (StatsEnemy[i,0] == Niveau)
                {
                    Vie = StatsEnemy[i, 1];
                    Dmg = StatsEnemy[i, 2];
                }
            }
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            float temps…coulÈ = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps…coulÈMAJ += temps…coulÈ;
            CaseEnnemi = new Case(true, new Point((int)(Position.X / Delta), (int)(Position.Z / Delta)));
            CasePlayer = new Case(true, new Point((int)(Player.Position.X / Delta), (int)(Player.Position.Z / Delta)));

            switch(…tat)
            {
                case ((byte)…tatEnnemi.RECHERCHE):
                    TrouverPath();
                    break;
                case ((byte)…tatEnnemi.ATTEINT):
                    ObjectifTrouver(gameTime);
                    break;
            }

            
            base.Update(gameTime);
        }

        private void TrouverPath()
        {
            if (DÈplacer && CaseEnnemi != CasePlayer)
            {
                Path = null;
                try
                {
                    foreach (Batiment b in Game.Components.OfType<Batiment>())
                    {
                        Grid.GridCase[(int)b.Position.X / Delta, (int)b.Position.Z / Delta].Accessible = false;
                    }
                }
                catch(Exception) { }
                if (BatimentCible == null)
                {
                    PathFinding.TrouverPath(new Vector2(Position.X, Position.Z), new Vector2((float)Math.Round(Player.Position.X, 0), (float)Math.Round(Player.Position.Z, 0)));
                    Path = PathFinding.Path;
                }
                if (Path == null)
                {
                    try
                    {
                        foreach (Batiment b in Game.Components.OfType<Batiment>())
                        {
                            Grid.GridCase[(int)b.Position.X / Delta, (int)b.Position.Z / Delta].Accessible = true;
                            if (Path == null)
                            {
                                PathFinding.TrouverPath(new Vector2(Position.X, Position.Z), new Vector2((float)Math.Round(b.Position.X, 0), (float)Math.Round(b.Position.Z, 0)));
                                Path = PathFinding.Path;
                                if (Path != null)
                                {
                                    CaseCible = new Case(true, new Point((int)(b.Position.X / Delta), (int)(b.Position.Z / Delta)));
                                    BatimentCible = b;
                                    CompteurDÈplacement = 40;
                                }
                            }
                        }
                    }
                    catch (Exception) { }
                }
                else
                {
                    CaseCible = CasePlayer;
                    CompteurDÈplacement = 40;
                }
            }

            if (CompteurDÈplacement >= 0)
            {
                if (Path != null && Path.Count != 0 && CaseEnnemi != CaseCible)
                {
                    IndexEnnemi = Path.IndexOf(Path.Find(c => c == CaseEnnemi));
                    CaseSuivante = Path[IndexEnnemi + 1];
                    if (CaseEnnemi.Position != CaseSuivante.Position)
                    {
                        Direction = new Vector3(CaseSuivante.Position.X * Delta - CaseEnnemi.Position.X * Delta, 0, CaseSuivante.Position.Y * Delta - CaseEnnemi.Position.Y * Delta);
                        Direction = Vector3.Normalize(Direction);
                        DÈplacement = new Vector3(Direction.X * 0.2f, 0, Direction.Z * 0.2f);
                    }
                    else
                    {
                        DÈplacement = Vector3.Zero;
                    }
                    Vector3 directionBase = Vector3.UnitX;
                    directionBase.Normalize();
                    double cosAngle = Vector3.Dot(Direction, directionBase);
                    if (Objectif.Z > Position.Z)
                    {
                        Angle = -(float)Math.Acos(cosAngle);
                    }
                    else
                    {
                        Angle = (float)Math.Acos(cosAngle);
                    }
                    Rotation = new Vector3(0, Angle, 0);
                }
                else
                {
                    DÈplacement = Vector3.Zero;
                }
                Distance = (float)Math.Sqrt(Math.Pow(Player.Position.X - Position.X + DÈplacement.X, 2) + Math.Pow(Player.Position.Z - Position.Z + DÈplacement.Z, 2));
                if (BatimentCible != null)
                {
                    float distanceBatiment = (float)Math.Sqrt(Math.Pow(BatimentCible.Position.X - Position.X + DÈplacement.X, 2) + Math.Pow(BatimentCible.Position.Z - Position.Z + DÈplacement.Z, 2));
                    if (distanceBatiment <= 3)
                    {
                        …tat = (byte)…tatEnnemi.ATTEINT;
                    }
                }
                else
                {
                    float distanceJoueur = (float)Math.Sqrt(Math.Pow(Player.Position.X - Position.X + DÈplacement.X, 2) + Math.Pow(Player.Position.Z - Position.Z + DÈplacement.Z, 2));
                    if (distanceJoueur <= 3)
                    {
                        …tat = (byte)…tatEnnemi.ATTEINT;
                    }
                }
                Position += DÈplacement;
                DÈplacer = false;
                CompteurDÈplacement--;
                Temps…coulÈMAJ = 0;
            }

            if (CompteurDÈplacement == -1 || DÈplacement == Vector3.Zero)
            {
                DÈplacer = true;
            }

            CalculerMonde();
        }

        private void ObjectifTrouver(GameTime gameTime)
        {
            float temps…coulÈ = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps…coulÈMAJ += temps…coulÈ;
            if (BatimentCible != null)
            {
                if (Temps…coulÈMAJ >= IntervalleAttaque)
                {
                    BatimentCible.NombrePtsDeVie -= Dmg;
                    if (BatimentCible.NombrePtsDeVie <= 0)
                    {
                        BatimentCible = null;
                    }
                    Temps…coulÈMAJ = 0;
                }
            }
            else
            {
                float distanceJoueur = (float)Math.Sqrt(Math.Pow(Player.Position.X - Position.X + DÈplacement.X, 2) + Math.Pow(Player.Position.Z - Position.Z + DÈplacement.Z, 2));
                if (distanceJoueur <= 5)
                {
                    if (Temps…coulÈMAJ >= IntervalleAttaque)
                    {
                        Player.NbPtsDeVie -= Dmg;
                        Temps…coulÈMAJ = 0;
                    }
                }
                else
                {
                    …tat = (byte)…tatEnnemi.RECHERCHE;
                }
            }
        }

        public void ToucherParBalle(int dmg)
        {
            Vie -= dmg;
            if (Vie <= 0)
            {
                int nombreParticuleSang = GenerateurAleatoire.Next(50, 100);
                for (int i = 0; i < nombreParticuleSang; i++)
                {
                    Vector3 direction = new Vector3(GenerateurAleatoire.Next(-100, 100), GenerateurAleatoire.Next(0, 100), GenerateurAleatoire.Next(-100, 100));
                    direction = Vector3.Normalize(direction);
                    Sang particuleSang = new Sang(Game, "blood", 0.01f, new Vector3(Position.X, Position.Y + 5, Position.Z), new Vector3(GenerateurAleatoire.Next(0, 360), GenerateurAleatoire.Next(0, 360), GenerateurAleatoire.Next(0, 360)), direction);
                    Game.Components.Add(particuleSang);
                }
                SoundDeath.Play();
                Dispose();
            }
        }

        enum …tatEnnemi
        {
            RECHERCHE,
            ATTEINT
        }
    }
}
