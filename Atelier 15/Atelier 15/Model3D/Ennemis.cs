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
        public int NumVague { get; set; }
        Vector3 Objectif { get; set; }
        float TempsÉcouléMAJ { get; set; }
        float Angle { get; set; }
        Vector2 PositionDépart { get; set; }
        public PathFindingAStar PathFinding { get; set; }
        public List<Case> Path { get; set; }
        Joueur Player { get; set; }
        Case CaseEnnemi { get; set; }
        Case CaseSuivante { get; set; }
        Case CasePlayer { get; set; }
        int IndexEnnemi { get; set; }
        int IndexAncien { get; set; }
        float Distance { get; set; }
        bool Déplacer { get; set; }
        Vector3 Déplacement { get; set; }
        int CompteurDéplacement { get; set; }
        bool Centré { get; set; }

        //Enemy stats
        int Niveau { get; set; }
        int[,] StatsEnemy = new int[12, 2];

        public Ennemis(Game game, string nomModele, float échelle, Vector3 position, Vector3 rotationInitiale, int niveau)
            : base(game, nomModele, échelle, position, rotationInitiale)
        {
            PathFinding = new PathFindingAStar(game);
            Niveau = niveau;
            StatsEnemy[0, 0] = 1;
            StatsEnemy[0, 1] = 5;
            StatsEnemy[1, 0] = 2;
            StatsEnemy[1, 1] = 10;
            StatsEnemy[2, 0] = 3;
            StatsEnemy[2, 1] = 15;
            StatsEnemy[3, 0] = 4;
            StatsEnemy[3, 1] = 20;
            StatsEnemy[4, 0] = 5;
            StatsEnemy[4, 1] = 25;
            StatsEnemy[5, 0] = 6;
            StatsEnemy[5, 1] = 50;
            StatsEnemy[6, 0] = 7;
            StatsEnemy[6, 1] = 75;
            StatsEnemy[7, 0] = 8;
            StatsEnemy[7, 1] = 100;
            StatsEnemy[8, 0] = 9;
            StatsEnemy[8, 1] = 150;
            StatsEnemy[9, 0] = 10;
            StatsEnemy[9, 1] = 200;
            StatsEnemy[10, 0] = 11;
            StatsEnemy[10, 1] = 250;
            StatsEnemy[11, 0] = 12;
            StatsEnemy[11, 1] = 500;
        }

        public override void Initialize()
        {
            foreach (Joueur j in Game.Components.OfType<Joueur>())
            {
                Player = j;
            }
            Centré = true;
            for (int i = 0; i < StatsEnemy.GetLength(0); i++)
            {
                if (StatsEnemy[i,0] == Niveau)
                {
                    Vie = StatsEnemy[i, 1];
                }
            }
            Dmg = 1;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléMAJ += tempsÉcoulé;
            CaseEnnemi = new Case(true, new Point((int)(Position.X / Delta), (int)(Position.Z / Delta)));
            CasePlayer = new Case(true, new Point((int)(Player.Position.X / Delta), (int)(Player.Position.Z / Delta)));

                
            //if (Déplacer && CaseEnnemi != CasePlayer)
            //{
            //    PathFinding.TrouverPath(new Vector2(Position.X, Position.Z), new Vector2((float)Math.Round(Player.Position.X, 0), (float)Math.Round(Player.Position.Z, 0)));
            //    Path = PathFinding.Path;
            //    CompteurDéplacement = 40;
            //}
            //else
            //{
            //    Déplacement = Vector3.Zero;
            //}


            if (CompteurDéplacement >= 0)
            {
                if (Path != null && Path.Count != 0 && CaseEnnemi != CasePlayer)
                {
                    IndexEnnemi = Path.IndexOf(Path.Find(c => c == CaseEnnemi));
                    CaseSuivante = Path[IndexEnnemi + 1];
                Vector3 direction = new Vector3(CaseSuivante.Position.X * Delta - CaseEnnemi.Position.X * Delta, 0, CaseSuivante.Position.Y * Delta - CaseEnnemi.Position.Y * Delta);
                    direction = Vector3.Normalize(direction);
                    Déplacement = new Vector3(direction.X * 0.1f, 0, direction.Z * 0.1f);
                    if (float.IsNaN(Déplacement.Y) || float.IsNaN(Déplacement.X))
                    {
                        string a = "a";
                    }
                Vector3 directionBase = Vector3.UnitX;
                directionBase.Normalize();
                    //double cosAngle = Vector3.Dot(direction, directionBase);
                    //if (Objectif.Z > Position.Z)
                    //{
                    //    Angle = -(float)Math.Acos(cosAngle);
                    //}
                    //else
                    //{
                    //    Angle = (float)Math.Acos(cosAngle);
                    //}
                    //Rotation = new Vector3(0, Angle, 0);
                }
                else
                {
                    Déplacement = Vector3.Zero;
                }
                Distance = (float)Math.Sqrt(Math.Pow(Player.Position.X - Position.X + Déplacement.X, 2) + Math.Pow(Player.Position.Z - Position.Z + Déplacement.Z, 2));
                Position += Déplacement;

                Déplacer = false;
                CompteurDéplacement--;
                TempsÉcouléMAJ = 0;
            }
            else
            {
                PathFinding.TrouverPath(new Vector2(Position.X, Position.Z), new Vector2((float)Math.Round(Player.Position.X, 0), (float)Math.Round(Player.Position.Z, 0)));
                Path = PathFinding.Path;
                CompteurDéplacement = 40;
            }
            CalculerMonde();
            base.Update(gameTime);
        }
    }
}
