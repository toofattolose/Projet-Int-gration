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
    public class Reparateur : BatimentRechargable
    {
        int Nombre�nergie { get; set; }
        int Nombre�nergieMaximum { get; set; }
        int NombrePtsDeVieEnvoy� { get; set; }
        int Nombre�nergieUtilis� { get; set; }
        float IntervalleEnvoieRecharge { get; set; }
        float Temps�coul�DepuisEnvoie { get; set; }

        public Reparateur(Game game, string nomModele, float �chelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModele,�chelle,position,rotationInitiale)
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
            Nombre�nergieMaximum = 1000;
            Nombre�nergie = Nombre�nergieMaximum;
            NombrePtsDeVieEnvoy� = 5;
            IntervalleEnvoieRecharge = 1;
            Nombre�nergieUtilis� = 1;
            Niveau = 1;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps�coul�DepuisEnvoie += temps�coul�;
            if (Temps�coul�DepuisEnvoie > IntervalleEnvoieRecharge)
            {
                R�parerBatiments();
                Temps�coul�DepuisEnvoie = 0;
            }
        }

        private void R�parerBatiments()
        {
            foreach (Batiment b in Game.Components.Where(x => x is Batiment))
            {
                if (b.NombrePtsDeVie < b.NombreMaxPtsDeVie && Nombre�nergie > Nombre�nergieUtilis�)
                {
                    float distance = (float)Math.Abs(Math.Sqrt(Math.Pow((Position.X - b.Position.X), 2) + Math.Pow((Position.Z - b.Position.Z), 2)));
                    if (distance < 20)
                    {
                        b.NombrePtsDeVie += NombrePtsDeVieEnvoy�;
                        Nombre�nergie -= Nombre�nergieUtilis�;
                        if (b.NombrePtsDeVie > b.NombreMaxPtsDeVie)
                        {
                            b.NombrePtsDeVie = NombreMaxPtsDeVie;
                        }
                    }
                }
            }
        }
    }
}
