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
        bool Centré { get; set; }
        Vector3 Direction { get; set; }
        Vector3 DirectionBase { get; set; }
        Vector3 Déplacement { get; set; }

        public Ennemis(Game game, string nomModele, float échelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModele, échelle, position, rotationInitiale)
        {
            PathFinding = new PathFindingAStar(game);
        }

        public override void Initialize()
        {
            foreach (Joueur j in Game.Components.OfType<Joueur>())
            {
                Player = j;
            }
            Centré = true;
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
            //EstCentré();

            //if (Centré)
            //{
                PathFinding.TrouverPath(new Vector2(Position.X, Position.Z), new Vector2(Player.Position.X, Player.Position.Z));
                Path = PathFinding.Path;
                IndexEnnemi = Path.IndexOf(Path.Find(c => c.Position == CaseEnnemi.Position));
                CaseSuivante = Path[IndexEnnemi + 1];
                
            //}

            if (TempsÉcouléMAJ >= 1 / 60f)
            {
                Vector3 direction = new Vector3(CaseSuivante.Position.X * Delta - CaseEnnemi.Position.X * Delta, 0, CaseSuivante.Position.Y * Delta - CaseEnnemi.Position.Y * Delta);
                Vector3 directionBase = Vector3.UnitX;
                direction.Normalize();
                directionBase.Normalize();
                Vector3 déplacement = new Vector3(direction.X * 0.1f, 0, direction.Z * 0.1f);
                double cosAngle = Vector3.Dot(Direction, DirectionBase);
                if (Objectif.Z > Position.Z)
                {
                    Angle = -(float)Math.Acos(cosAngle);
                }
                else
                {
                    Angle = (float)Math.Acos(cosAngle);
                }

                Distance = (float)Math.Sqrt(Math.Pow(Player.Position.X - Position.X + Déplacement.X, 2) + Math.Pow(Player.Position.Y - Position.Y + Déplacement.Y, 2));
                Rotation = new Vector3(0, Angle, 0);
                if (Distance >= 1)
                {
                    Position += déplacement;
                }

                TempsÉcouléMAJ = 0;
            }

            //else
            //{
            //    --Player.Vie;
            //}

            CalculerMonde();
            base.Update(gameTime);
        }

        void EstCentré()
        {
            if (Position.X == CaseEnnemi.Position.X * Delta + 2 && Position.Z == CaseEnnemi.Position.Y * Delta + 2)
            {
                Centré = true;
            }
            else
            {
                Centré = false;
            }
        }
    }
}
