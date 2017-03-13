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
    public class Ennemis : Model3D
    {
        const int Delta = 4;
        public float Vie { get; set; }
        public float Dmg { get; set; }
        public int NumVague { get; set; }
        Vector3 Objectif { get; set; }
        float Temps…coulÈMAJ { get; set; }
        float Angle { get; set;}
        Vector2 PositionDÈpart { get; set; }
        public PathFindingAStar PathFinding { get; set; }
        public List<Case> Path { get; set; }
        Joueur Player { get; set; }
        Case CaseEnnemi { get; set; }
        Case CaseSuivante { get; set; }

        public Ennemis(Game game, string nomModele, float Èchelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModele, Èchelle, position, rotationInitiale)
        {
            PathFinding = new PathFindingAStar(game);
        }

        public override void Initialize()
        {
            foreach(Joueur j in Game.Components.OfType<Joueur>())
            {
                Player = j;
            }
            Vie = 1f;
            Dmg = 1f;
            base.Initialize();
        }
        
        public override void Update(GameTime gameTime)
        {
            float temps…coulÈ = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps…coulÈMAJ += temps…coulÈ;
            //PathFinding.TrouverPath(new Vector2(Position.X, Position.Z), new Vector2(Player.Position.X, Player.Position.Z));
            //Path = PathFinding.Path;
            //CaseEnnemi = new Case(true, new Point((int)(Position.X / Delta), (int)(Position.Z / Delta)));

            //int indexEnnemi = Path.IndexOf(Path.Find(c => c.Position == CaseEnnemi.Position));
            //CaseSuivante = Path[indexEnnemi + 1];

            //if (Temps…coulÈMAJ >= 1 / 60f)
            //{
            //    Vector3 direction = new Vector3(CaseSuivante.Position.X * Delta - CaseEnnemi.Position.X * Delta, 0, CaseSuivante.Position.Y * Delta - CaseEnnemi.Position.Y * Delta);
            //    Vector3 directionBase = Vector3.UnitX;
            //    direction.Normalize();
            //    directionBase.Normalize();
            //    Vector3 dÈplacement = new Vector3(direction.X * 0.05f, 0, direction.Z * 0.05f);
            //    double cosAngle = Vector3.Dot(direction, directionBase);
            //    if (Objectif.Z > Position.Z)
            //    {
            //        Angle = -(float)Math.Acos(cosAngle);
            //    }
            //    else
            //    {
            //        Angle = (float)Math.Acos(cosAngle);
            //    }
            //    Rotation = new Vector3(0, Angle, 0);
            //    Position += dÈplacement;
            //    Temps…coulÈMAJ = 0;
            //}
            CalculerMonde();
            base.Update(gameTime);
        }
    }
}
