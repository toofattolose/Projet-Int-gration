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
        public float Vie { get; set; }
        public float Dmg { get; set; }
        public int NumVague { get; set; }
        Vector3 Objectif { get; set; }
        float TempsÉcouléMAJ { get; set; }
        float Angle { get; set;}

        public Ennemis(Game game, string nomModele, float échelle, Vector3 position, Vector3 rotationInitiale, int numVague)
            : base(game, nomModele, échelle, position, rotationInitiale)
        {
            NumVague = numVague;
        }

        public override void Initialize()
        {
            Vie = NumVague;
            Dmg = 1f;
            base.Initialize();
        }
        
        public override void Update(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléMAJ += tempsÉcoulé;
            foreach(Joueur j in Game.Components.OfType<Joueur>())
            {
                Objectif = j.Position;
            }

            if (TempsÉcouléMAJ >= 1 / 60f)
            {
                Vector3 direction = new Vector3(Objectif.X - Position.X, 0, Objectif.Z - Position.Z);
                Vector3 directionBase = Vector3.UnitX;
                direction.Normalize();
                directionBase.Normalize();
                Vector3 déplacement = new Vector3(direction.X * 0.05f, 0, direction.Z * 0.05f);
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
                Position += déplacement;
                TempsÉcouléMAJ = 0;
            }
            CalculerMonde();
            base.Update(gameTime);
        }
    }
}
