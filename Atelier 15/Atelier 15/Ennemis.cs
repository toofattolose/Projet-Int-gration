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
        public float Vie { get; set; }
        public float Dmg { get; set; }
        public int NumVague { get; set; }
        Vector3 Objectif { get; set; }
        float Temps�coul�MAJ { get; set; }
        float Angle { get; set;}
        Vector2 PositionD�part { get; set; }
        public PathFindingAStar PathFinding { get; set; }
        public List<Case> Path { get; set; }
        Joueur Player { get; set; }
        Case CaseEnnemi { get; set; }
        Case CaseSuivante { get; set; }

        public Ennemis(Game game, string nomModele, float �chelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModele, �chelle, position, rotationInitiale)
        {
            PositionD�part = new Vector2(position.X, position.Z);
            PathFinding = new PathFindingAStar(game);
        }

        public override void Initialize()
        {
            foreach(Joueur j in Game.Components)
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
            PathFinding.TrouverPath(PositionD�part, new Vector2(Player.Position.X, Player.Position.Z));
            Path = PathFinding.Path;
            CaseEnnemi = new Case(true, new Point((int)Math.Floor(Position.X), (int)Math.Floor(Position.Z)));

            int indexEnnemi = Path.IndexOf(Path.Find(c => c == CaseEnnemi));
            CaseSuivante = Path[indexEnnemi + 1];

            if (Temps�coul�MAJ >= 1 / 60f)
            {
                Vector3 direction = new Vector3(CaseSuivante.Position.X - CaseEnnemi.Position.X, 0, CaseSuivante.Position.Y - CaseEnnemi.Position.Y);
                Vector3 directionBase = Vector3.UnitX;
                direction.Normalize();
                directionBase.Normalize();
                Vector3 d�placement = new Vector3(direction.X * 0.05f, 0, direction.Z * 0.05f);
                double cosAngle = Vector3.Dot(direction, directionBase);
                if(Objectif.Z > Position.Z)
                {
                    Angle = -(float)Math.Acos(cosAngle);
                }
                else
                {
                    Angle = (float)Math.Acos(cosAngle);
                }
                Rotation = new Vector3(0, Angle, 0);
                Position += d�placement;
                Temps�coul�MAJ = 0;
            }
            CalculerMonde();
            base.Update(gameTime);
        }
    }
}
