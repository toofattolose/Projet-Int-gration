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
        float Temps�coul�DepuisMAJ { get; set; }
        float IntervalleMAJ { get; set; }
        float IntervalleDisparition { get; set; }
        byte �tat { get; set; }
        Random Generateur { get; set; }

        public Sang(Game game, string nomModele, float �chelle, Vector3 position, Vector3 rotationInitiale, Vector3 direction)
            : base(game, nomModele, �chelle, position, rotationInitiale)
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
            �tat = (byte)�tatSang.MOUVEMENT;
        }

        public override void Update(GameTime gameTime)
        {
            switch(�tat)
            {
                case (byte)�tatSang.MOUVEMENT:
                    G�rerMouvement(gameTime);
                    break;
                case (byte)�tatSang.ARRET:
                    G�rerArret(gameTime);
                    break;
            }
            base.Update(gameTime);
            
        }

        private void G�rerMouvement(GameTime gameTime)
        {
            float temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps�coul�DepuisMAJ += temps�coul�;
            if (Temps�coul�DepuisMAJ >= IntervalleMAJ)
            {
                Position += Direction * Vitesse;
                Position += new Vector3(0, -ACCELERATION, 0);
                if (Position.Y <= 0)
                {
                    Position = new Vector3(Position.X, 0, Position.Z);
                    �tat = (byte)�tatSang.ARRET;
                }
                CalculerMonde();
                Temps�coul�DepuisMAJ = 0;
            }
        }

        private void G�rerArret(GameTime gameTime)
        {
            float temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps�coul�DepuisMAJ += temps�coul�;
            if (Temps�coul�DepuisMAJ >= IntervalleDisparition)
            {
                Dispose();
                Temps�coul�DepuisMAJ = 0;
            }
        }

        enum �tatSang
        {
            MOUVEMENT,
            ARRET
        }
    }
}
