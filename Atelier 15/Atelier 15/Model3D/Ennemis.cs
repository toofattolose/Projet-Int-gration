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
        float Temps�coul�MAJ { get; set; }
        float Angle { get; set; }
        Vector2 PositionD�part { get; set; }
        public PathFindingAStar PathFinding { get; set; }
        public List<Case> Path { get; set; }
        Joueur Player { get; set; }
        Case CaseEnnemi { get; set; }
        Case CaseSuivante { get; set; }
        Case CasePlayer { get; set; }
        int IndexEnnemi { get; set; }
        int IndexAncien { get; set; }
        float Distance { get; set; }
        bool D�placer { get; set; }
        Vector3 D�placement { get; set; }
        int CompteurD�placement { get; set; }

        public Ennemis(Game game, string nomModele, float �chelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModele, �chelle, position, rotationInitiale)
        {
            PathFinding = new PathFindingAStar(game);
            D�placer = true;
            CompteurD�placement = -1;
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
            float temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps�coul�MAJ += temps�coul�;
            CaseEnnemi = new Case(true, new Point((int)(Position.X / Delta), (int)(Position.Z / Delta)));
            CasePlayer = new Case(true, new Point((int)(Player.Position.X / Delta), (int)(Player.Position.Z / Delta)));


            //if (D�placer && CaseEnnemi != CasePlayer)
            //{
            //    PathFinding.TrouverPath(new Vector2(Position.X, Position.Z), new Vector2((float)Math.Round(Player.Position.X, 0), (float)Math.Round(Player.Position.Z, 0)));
            //    Path = PathFinding.Path;
            //    CompteurD�placement = 40;
            //}
            //else
            //{
            //    D�placement = Vector3.Zero;
            //}


            if (CompteurD�placement >= 0)
            {
                if (Path != null && Path.Count != 0 && CaseEnnemi != CasePlayer)
                {
                    CaseSuivante = Path[0];
                    Vector3 direction = new Vector3(CaseSuivante.Position.X * Delta - CaseEnnemi.Position.X * Delta, 0, CaseSuivante.Position.Y * Delta - CaseEnnemi.Position.Y * Delta);
                    direction.Normalize();
                    D�placement = new Vector3(direction.X * 0.1f, 0, direction.Z * 0.1f);
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
                    D�placement = Vector3.Zero;
                }
                Distance = (float)Math.Sqrt(Math.Pow(Player.Position.X - Position.X + D�placement.X, 2) + Math.Pow(Player.Position.Z - Position.Z + D�placement.Z, 2));
                Position += D�placement;
                D�placer = false;
                CompteurD�placement--;
                Temps�coul�MAJ = 0;
            }
            else
            {
                try
                {
                    float y = Position.X + Position.Z;
                }
                catch { Exception e; }
            
                PathFinding.TrouverPath(new Vector2(Position.X, Position.Z), new Vector2((float)Math.Round(Player.Position.X, 0), (float)Math.Round(Player.Position.Z, 0)));
                Path = PathFinding.Path;
                CompteurD�placement = 40;
            }
            CalculerMonde();
            base.Update(gameTime);
        }
    }
}
