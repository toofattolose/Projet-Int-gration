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
        public float[,] TableauValeurNiveau = new float[10, 7];

        public Generatrice(Game game, string nomModele, float Èchelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModele,Èchelle,position,rotationInitiale)
        {
            // TODO: Construct any child components here
            TableauValeurNiveau[0, 0] = 1; //niveau
            TableauValeurNiveau[0, 1] = 100; //nb vie
            TableauValeurNiveau[0, 2] = 150; //cout bois
            TableauValeurNiveau[0, 3] = 75; //cout or
            TableauValeurNiveau[0, 4] = 1; // Nombre Ènergie envoie
            TableauValeurNiveau[0, 5] = 2; // Nombre …nergie Recharge
            TableauValeurNiveau[0, 6] = 100; // Nombre …nergie Max

            TableauValeurNiveau[1, 0] = 2;
            TableauValeurNiveau[1, 1] = 150;// nb Pts de vie
            TableauValeurNiveau[1, 2] = 450; //cout bois
            TableauValeurNiveau[1, 3] = 225; //cout or
            TableauValeurNiveau[1, 4] = 2; // Nombre Ènergie envoie
            TableauValeurNiveau[1, 5] = 4; // Nombre …nergie Recharge
            TableauValeurNiveau[1, 6] = 200; // Nombre …nergie Max

            TableauValeurNiveau[2, 0] = 3;
            TableauValeurNiveau[2, 1] = 200;
            TableauValeurNiveau[2, 2] = 1350; //cout bois
            TableauValeurNiveau[2, 3] = 675; //cout or
            TableauValeurNiveau[2, 4] = 5; // Nombre Ènergie envoie
            TableauValeurNiveau[2, 5] = 10; // Nombre …nergie Recharge
            TableauValeurNiveau[2, 6] = 500; // Nombre …nergie Max

            TableauValeurNiveau[3, 0] = 4;
            TableauValeurNiveau[3, 1] = 250;
            TableauValeurNiveau[3, 2] = 4050; //cout bois
            TableauValeurNiveau[3, 3] = 2025; //cout or
            TableauValeurNiveau[3, 4] = 10; // Nombre Ènergie envoie
            TableauValeurNiveau[3, 5] = 20; // Nombre …nergie Recharge
            TableauValeurNiveau[3, 6] = 1000; // Nombre …nergie Max

            TableauValeurNiveau[4, 0] = 5;
            TableauValeurNiveau[4, 1] = 300;
            TableauValeurNiveau[4, 2] = 12150; //cout bois
            TableauValeurNiveau[4, 3] = 6075; //cout or
            TableauValeurNiveau[4, 4] = 20; // Nombre Ènergie envoie
            TableauValeurNiveau[4, 5] = 40; // Nombre …nergie Recharge
            TableauValeurNiveau[4, 6] = 2000; // Nombre …nergie Max

            TableauValeurNiveau[5, 0] = 6;
            TableauValeurNiveau[5, 1] = 350;
            TableauValeurNiveau[5, 2] = 36450; //cout bois
            TableauValeurNiveau[5, 3] = 18225; //cout or
            TableauValeurNiveau[5, 4] = 50; // Nombre Ènergie envoie
            TableauValeurNiveau[5, 5] = 100; // Nombre …nergie Recharge
            TableauValeurNiveau[5, 6] = 5000; // Nombre …nergie Max

            TableauValeurNiveau[6, 0] = 7;
            TableauValeurNiveau[6, 1] = 400;
            TableauValeurNiveau[6, 2] = 109350; //cout bois
            TableauValeurNiveau[6, 3] = 54675; //cout or
            TableauValeurNiveau[6, 4] = 100; // Nombre Ènergie envoie
            TableauValeurNiveau[6, 5] = 200; // Nombre …nergie Recharge
            TableauValeurNiveau[6, 6] = 10000; // Nombre …nergie Max

            TableauValeurNiveau[7, 0] = 8;
            TableauValeurNiveau[7, 1] = 450;
            TableauValeurNiveau[7, 2] = 328050; //cout bois
            TableauValeurNiveau[7, 3] = 164025; //cout or
            TableauValeurNiveau[7, 4] = 200; // Nombre Ènergie envoie
            TableauValeurNiveau[7, 5] = 400; // Nombre …nergie Recharge
            TableauValeurNiveau[7, 6] = 20000; // Nombre …nergie Max

            TableauValeurNiveau[8, 0] = 9;
            TableauValeurNiveau[8, 1] = 500;
            TableauValeurNiveau[8, 2] = 984150; //cout bois
            TableauValeurNiveau[8, 3] = 492075; //cout or
            TableauValeurNiveau[8, 4] = 500; // Nombre Ènergie envoie
            TableauValeurNiveau[8, 5] = 1000; // Nombre …nergie Recharge
            TableauValeurNiveau[8, 6] = 50000; // Nombre …nergie Max

            TableauValeurNiveau[9, 0] = 10;
            TableauValeurNiveau[9, 1] = 550;
            TableauValeurNiveau[9, 2] = 0; //cout bois
            TableauValeurNiveau[9, 3] = 0; //cout or
            TableauValeurNiveau[9, 4] = 1000; // Nombre Ènergie envoie
            TableauValeurNiveau[9, 5] = 2000; // Nombre …nergie Recharge
            TableauValeurNiveau[9, 6] = 100000; // Nombre …nergie Max
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            NombreMaxPtsDeVie = (int)TableauValeurNiveau[0,2];
            NombrePtsDeVie = NombreMaxPtsDeVie;
            Nombre…nergieMaximum = (int)TableauValeurNiveau[0,6];
            Nombre…nergie = Nombre…nergieMaximum;
            Nombre…nergieEnvoyÈ = (int)TableauValeurNiveau[0,4];
            Nombre…nergieRecharge = (int)TableauValeurNiveau[0,5];
            IntervalleEnvoieRecharge = 0.5f;
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
                        if (j.NombreDeBois >= TableauValeurNiveau[i, 2] && j.NombreDOR >= TableauValeurNiveau[i, 3])
                        {
                            SoundUpgrade.Play();
                            ++Niveau;
                            NombreMaxPtsDeVie = (int)TableauValeurNiveau[i + 1, 1];
                            NombrePtsDeVie = (int)TableauValeurNiveau[i + 1, 1];
                            Nombre…nergieMaximum = (int)TableauValeurNiveau[i + 1, 6];
                            Nombre…nergie = Nombre…nergieMaximum;
                            Nombre…nergieEnvoyÈ = (int)TableauValeurNiveau[i + 1, 4];
                            Nombre…nergieRecharge = (int)TableauValeurNiveau[i + 1, 5];
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
