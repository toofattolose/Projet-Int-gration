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
        float Temps�coul�DepuisMAJ { get; set; }
        float IntervalleMAJ { get; set; }
        Vector3 Direction { get; set; }

        public BalleJoueur(Game game, string nomModele, float �chelle, Vector3 position, Vector3 rotationInitiale, int dommage, Vector3 direction, float intervalleMAJ)
            : base(game, nomModele, �chelle, position, rotationInitiale)
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
            float temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps�coul�DepuisMAJ += temps�coul�;
            if (Temps�coul�DepuisMAJ >= IntervalleMAJ)
            {
                G�rerD�placement();
                CalculerMonde();
                G�rerCollision();
                Temps�coul�DepuisMAJ = 0;
            }
            if (temps�coul� >= 3)
            {
                Dispose();
            }
            base.Update(gameTime);
        }

        private void G�rerD�placement()
        {
            Position += Direction * Vitesse;
        }

        private void G�rerCollision()
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
