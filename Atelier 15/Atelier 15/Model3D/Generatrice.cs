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
        public int Nombre…nergie { get; set; }
        public int Nombre…nergieMaximum { get; set; }
        int Nombre…nergieEnvoyÈ { get; set; }
        int Nombre…nergieRecharge { get; set; }
        float IntervalleEnvoieRecharge { get; set; }
        float Temps…coulÈDepuisEnvoie { get; set; }
        public float[,] TableauValeurNiveau = new float[10, 4];

        public Generatrice(Game game, string nomModele, float Èchelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModele,Èchelle,position,rotationInitiale)
        {
            // TODO: Construct any child components here
            TableauValeurNiveau[0, 0] = 1; //niveau
            TableauValeurNiveau[0, 1] = 1000; //nb vie
            TableauValeurNiveau[0, 2] = 150; //cout bois
            TableauValeurNiveau[0, 3] = 75; //cout or

            TableauValeurNiveau[1, 0] = 2;
            TableauValeurNiveau[1, 1] = 1500;
            TableauValeurNiveau[1, 2] = 350; //cout bois
            TableauValeurNiveau[1, 3] = 175; //cout or

            TableauValeurNiveau[2, 0] = 3;
            TableauValeurNiveau[2, 1] = 2500;
            TableauValeurNiveau[2, 2] = 500; //cout bois
            TableauValeurNiveau[2, 3] = 250; //cout or

            TableauValeurNiveau[3, 0] = 4;
            TableauValeurNiveau[3, 1] = 5000;
            TableauValeurNiveau[3, 2] = 1000; //cout bois
            TableauValeurNiveau[3, 3] = 500; //cout or

            TableauValeurNiveau[4, 0] = 5;
            TableauValeurNiveau[4, 1] = 10000;
            TableauValeurNiveau[4, 2] = 2500; //cout bois
            TableauValeurNiveau[4, 3] = 1250; //cout or

            TableauValeurNiveau[5, 0] = 6;
            TableauValeurNiveau[5, 1] = 25000;
            TableauValeurNiveau[5, 2] = 5000; //cout bois
            TableauValeurNiveau[5, 3] = 2500; //cout or

            TableauValeurNiveau[6, 0] = 7;
            TableauValeurNiveau[6, 1] = 50000;
            TableauValeurNiveau[6, 2] = 10000; //cout bois
            TableauValeurNiveau[6, 3] = 5000; //cout or

            TableauValeurNiveau[7, 0] = 8;
            TableauValeurNiveau[7, 1] = 100000;
            TableauValeurNiveau[7, 2] = 25000; //cout bois
            TableauValeurNiveau[7, 3] = 12500; //cout or

            TableauValeurNiveau[8, 0] = 9;
            TableauValeurNiveau[8, 1] = 250000;
            TableauValeurNiveau[8, 2] = 50000; //cout bois
            TableauValeurNiveau[8, 3] = 25000; //cout or

            TableauValeurNiveau[9, 0] = 10;
            TableauValeurNiveau[9, 1] = 1000000;
            TableauValeurNiveau[9, 2] = 100000; //cout bois
            TableauValeurNiveau[9, 3] = 50000; //cout or
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            NombreMaxPtsDeVie = 100;
            NombrePtsDeVie = NombreMaxPtsDeVie;
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

        public void MonterDeNiveau()
        {
            bool estUpgrader = false;
            foreach (Joueur j in Game.Components.OfType<Joueur>())
            {
                for (int i = 0; i < TableauValeurNiveau.GetLength(0); i++)
                {
                    if (Niveau == TableauValeurNiveau[i, 0] && Niveau != 10 && !estUpgrader)
                    {
                        if (j.NombreDeBois >= TableauValeurNiveau[i + 1, 2] && j.NombreDOR >= TableauValeurNiveau[i + 1, 3])
                        {
                            SoundUpgrade.Play();
                            ++Niveau;
                            NombreMaxPtsDeVie = (int)TableauValeurNiveau[i + 1, 1];
                            NombrePtsDeVie = (int)TableauValeurNiveau[i + 1, 1];
                            j.NombreDeBois -= (int)TableauValeurNiveau[i + 1, 2];
                            j.NombreDOR -= (int)TableauValeurNiveau[i + 1, 3];
                            estUpgrader = true;
                        }
                    }
                }
            }
        }

    }
}
