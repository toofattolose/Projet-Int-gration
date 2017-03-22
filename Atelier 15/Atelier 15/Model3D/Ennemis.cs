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
        public float Vie { get; set; }
        public float Dmg { get; set; }
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

        public Ennemis(Game game, string nomModele, float échelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModele, échelle, position, rotationInitiale)
        {
            PathFinding = new PathFindingAStar(game);
            Déplacer = true;
            CompteurDéplacement = -1;
        }

        public override void Initialize()
        {
            foreach (Joueur j in Game.Components.OfType<Joueur>())
            {
                Player = j;
            }
            Vie = 1f;
            Dmg = 1f;
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
