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
    public class Sang : Model3D
    {
        const float ACCELERATION = 0.5f;
        float Vitesse { get; set; }
        Vector3 Direction { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        float IntervalleMAJ { get; set; }
        float IntervalleDisparition { get; set; }
        byte État { get; set; }
        Random Generateur { get; set; }

        public Sang(Game game, string nomModele, float échelle, Vector3 position, Vector3 rotationInitiale, Vector3 direction)
            : base(game, nomModele, échelle, position, rotationInitiale)
        {
            Direction = direction;
        }

        public override void Initialize()
        {
            base.Initialize();
            IntervalleMAJ = 1 / 60f;
            IntervalleDisparition = 1f;
            Generateur = Game.Services.GetService(typeof(Random)) as Random;
            Vitesse = Generateur.Next(1, 51);
            Vitesse = Vitesse / 100f;
            État = (byte)ÉtatSang.MOUVEMENT;
        }

        public override void Update(GameTime gameTime)
        {
            switch(État)
            {
                case (byte)ÉtatSang.MOUVEMENT:
                    GérerMouvement(gameTime);
                    break;
                case (byte)ÉtatSang.ARRET:
                    GérerArret(gameTime);
                    break;
            }
            base.Update(gameTime);
            
        }

        private void GérerMouvement(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                Position += Direction * Vitesse;
                Position += new Vector3(0, -ACCELERATION, 0);
                if (Position.Y <= 0)
                {
                    Position = new Vector3(Position.X, 0, Position.Z);
                    État = (byte)ÉtatSang.ARRET;
                }
                CalculerMonde();
                TempsÉcouléDepuisMAJ = 0;
            }
        }

        private void GérerArret(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleDisparition)
            {
                Dispose();
                TempsÉcouléDepuisMAJ = 0;
            }
        }

        enum ÉtatSang
        {
            MOUVEMENT,
            ARRET
        }
    }
}
