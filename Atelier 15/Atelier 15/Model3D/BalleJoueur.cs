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
    public class BalleJoueur : Model3DAvecCollision
    {
        float Vitesse { get; set; }
        int Dommage { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        float IntervalleMAJ { get; set; }
        Vector3 Direction { get; set; }

        public BalleJoueur(Game game, string nomModele, float échelle, Vector3 position, Vector3 rotationInitiale, int dommage, Vector3 direction, float intervalleMAJ)
            : base(game, nomModele, échelle, position, rotationInitiale)
        {
            Dommage = dommage;
            IntervalleMAJ = intervalleMAJ;
            Direction = direction;
        }


        public override void Initialize()
        {
            Vitesse = 2f;
            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                GérerDéplacement();
                CalculerMonde();
                GérerCollision();
                TempsÉcouléDepuisMAJ = 0;
            }
            if (tempsÉcoulé >= 3)
            {
                Dispose();
            }
            base.Update(gameTime);
        }

        private void GérerDéplacement()
        {
            Position += Direction * Vitesse;
        }

        private void GérerCollision()
        {
            try
            {
                foreach(Ennemis e in Game.Components.OfType<Ennemis>())
                {
                    if (EstEnCollision(e))
                    {
                        Dispose();
                    }
                }
            }
            catch (Exception) { }
        }
    }
}
