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
    public class Generatrice : Batiment
    {
        int Nombre…nergie { get; set; }
        int Nombre…nergieMaximum { get; set; }
        int Nombre…nergieEnvoyÈ { get; set; }
        int Nombre…nergieRecharge { get; set; }
        float IntervalleEnvoieRecharge { get; set; }
        int Niveau { get; set; }
        float Temps…coulÈDepuisEnvoie { get; set; }

        public Generatrice(Game game, string nomModele, float Èchelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModele,Èchelle,position,rotationInitiale)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            NombrePtsDeVie = 100;
            Nombre…nergieMaximum = 1000;
            Nombre…nergie = Nombre…nergieMaximum;
            Nombre…nergieEnvoyÈ = 1;
            Nombre…nergieRecharge = 2;
            IntervalleEnvoieRecharge = 1;
            Niveau = 1;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float temps…coulÈ = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps…coulÈDepuisEnvoie += temps…coulÈ;
            if (Temps…coulÈDepuisEnvoie > IntervalleEnvoieRecharge)
            {
                RechargerBatiments();
                Temps…coulÈDepuisEnvoie = 0;
            }
        }

        private void RechargerBatiments()
        {
            foreach (BatimentRechargable b in Game.Components.Where(x => x is BatimentRechargable))
            {
                if (b.NombrePtsEnergie < b.NombreMaxEnergie && Nombre…nergie > 0)
                {
                    float distance = (float)Math.Abs(Math.Sqrt(Math.Pow((Position.X - b.Position.X),2) + Math.Pow((Position.Z - b.Position.Z),2)));
                    if (distance < 20)
                    {
                        b.NombrePtsEnergie += Nombre…nergieEnvoyÈ;
                        Nombre…nergie -= Nombre…nergieEnvoyÈ;
                    }   
                }
            }
            if (Nombre…nergie < Nombre…nergieMaximum)
            {
                Nombre…nergie += Nombre…nergieRecharge;
                if (Nombre…nergie > Nombre…nergieMaximum)
                {
                    Nombre…nergie = Nombre…nergieMaximum;
                }
            }
        }

    }
}
