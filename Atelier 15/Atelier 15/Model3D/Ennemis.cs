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
        int NombrePts { get; set; }

        float IntervalleAttaque { get; set; }

        //Enemy stats
        int Niveau { get; set; }
        int[,] StatsEnemy = new int[12, 4];

        public Ennemis(Game game, string nomModele, float Èchelle, Vector3 position, Vector3 rotationInitiale, int niveau)
            : base(game, nomModele, Èchelle, position, rotationInitiale)
        {
            PathFinding = new PathFindingAStar(game);
            Niveau = niveau;
            DÈplacer = true;
            StatsEnemy[0, 0] = 1; //Niveau
            StatsEnemy[0, 1] = 5; //Vie
            StatsEnemy[0, 2] = 1; //Dommage
            StatsEnemy[0, 3] = 1; //Nb Points par kill

            StatsEnemy[1, 0] = 2;
            StatsEnemy[1, 1] = 10;
            StatsEnemy[1, 2] = 2; //Dommage
            StatsEnemy[1, 3] = 4; //Nb Points par kill

            StatsEnemy[2, 0] = 3;
            StatsEnemy[2, 1] = 25;
            StatsEnemy[2, 2] = 4; //Dommage
            StatsEnemy[2, 3] = 16; //Nb Points par kill

            StatsEnemy[3, 0] = 4;
            StatsEnemy[3, 1] = 50;
            StatsEnemy[3, 2] = 8; //Dommage
            StatsEnemy[3, 3] = 64; //Nb Points par kill

            StatsEnemy[4, 0] = 5;
            StatsEnemy[4, 1] = 125;
            StatsEnemy[4, 2] = 16; //Dommage
            StatsEnemy[4, 3] = 256; //Nb Points par kill

            StatsEnemy[5, 0] = 6;
            StatsEnemy[5, 1] = 250;
            StatsEnemy[5, 2] = 32; //Dommage
            StatsEnemy[5, 3] = 1024; //Nb Points par kill

            StatsEnemy[6, 0] = 7;
            StatsEnemy[6, 1] = 500;
            StatsEnemy[6, 2] = 64; //Dommage
            StatsEnemy[6, 3] = 4096; //Nb Points par kill

            StatsEnemy[7, 0] = 8;
            StatsEnemy[7, 1] = 1500;
            StatsEnemy[7, 2] = 128; //Dommage
            StatsEnemy[7, 3] = 16384; //Nb Points par kill

            StatsEnemy[8, 0] = 9;
            StatsEnemy[8, 1] = 3000;
            StatsEnemy[8, 2] = 256; //Dommage
            StatsEnemy[8, 3] = 65536; //Nb Points par kill

            StatsEnemy[9, 0] = 10;
            StatsEnemy[9, 1] = 7500;
            StatsEnemy[9, 2] = 512; //Dommage
            StatsEnemy[9, 3] = 262144; //Nb Points par kill

            StatsEnemy[10, 0] = 11;
            StatsEnemy[10, 1] = 12500;
            StatsEnemy[10, 2] = 1024; //Dommage
            StatsEnemy[10, 3] = 1048576; //Nb Points par kill

            StatsEnemy[11, 0] = 12;
            StatsEnemy[11, 1] = 20000;
            StatsEnemy[11, 2] = 2048; //Dommage
            StatsEnemy[11, 3] = 4194304; //Nb Points par kill
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
                    NombrePts = StatsEnemy[i, 3];
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
