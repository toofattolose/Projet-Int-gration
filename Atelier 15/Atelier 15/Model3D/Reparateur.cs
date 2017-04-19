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
        int NombrePtsDeVieEnvoyé { get; set; }
        int NombreÉnergieUtilisé { get; set; }
        float IntervalleEnvoieRecharge { get; set; }
        float TempsÉcouléDepuisEnvoie { get; set; }
        public float[,] TableauValeurNiveau = new float[10, 7];

        public Reparateur(Game game, string nomModele, float échelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModele,échelle,position,rotationInitiale)
        {
            // TODO: Construct any child components here
            TableauValeurNiveau[0, 0] = 1; //niveau
            TableauValeurNiveau[0, 1] = 1000; //nb vie
            TableauValeurNiveau[0, 2] = 150; //cout bois
            TableauValeurNiveau[0, 3] = 75; //cout or
            TableauValeurNiveau[0, 4] = 100; // Max Énergie
            TableauValeurNiveau[0, 5] = 10; //Pts de vie envoyer
            TableauValeurNiveau[0, 6] = 2; // Nb Énergie utilisé

            TableauValeurNiveau[1, 0] = 2;
            TableauValeurNiveau[1, 1] = 1500;
            TableauValeurNiveau[1, 2] = 350; //cout bois
            TableauValeurNiveau[1, 3] = 175; //cout or
            TableauValeurNiveau[1, 4] = 200; // Max Énergie
            TableauValeurNiveau[1, 5] = 20; //Pts de vie envoyer
            TableauValeurNiveau[1, 6] = 4; // Nb Énergie utilisé

            TableauValeurNiveau[2, 0] = 3;
            TableauValeurNiveau[2, 1] = 2500;
            TableauValeurNiveau[2, 2] = 500; //cout bois
            TableauValeurNiveau[2, 3] = 250; //cout or
            TableauValeurNiveau[2, 4] = 500; // Max Énergie
            TableauValeurNiveau[2, 5] = 50; //Pts de vie envoyer
            TableauValeurNiveau[2, 6] = 10; // Nb Énergie utilisé

            TableauValeurNiveau[3, 0] = 4;
            TableauValeurNiveau[3, 1] = 5000;
            TableauValeurNiveau[3, 2] = 1000; //cout bois
            TableauValeurNiveau[3, 3] = 500; //cout or
            TableauValeurNiveau[3, 4] = 1000; // Max Énergie
            TableauValeurNiveau[3, 5] = 100; //Pts de vie envoyer
            TableauValeurNiveau[3, 6] = 20; // Nb Énergie utilisé

            TableauValeurNiveau[4, 0] = 5;
            TableauValeurNiveau[4, 1] = 10000;
            TableauValeurNiveau[4, 2] = 2500; //cout bois
            TableauValeurNiveau[4, 3] = 1250; //cout or
            TableauValeurNiveau[4, 4] = 2000; // Max Énergie
            TableauValeurNiveau[4, 5] = 200; //Pts de vie envoyer
            TableauValeurNiveau[4, 6] = 40; // Nb Énergie utilisé

            TableauValeurNiveau[5, 0] = 6;
            TableauValeurNiveau[5, 1] = 25000;
            TableauValeurNiveau[5, 2] = 5000; //cout bois
            TableauValeurNiveau[5, 3] = 2500; //cout or
            TableauValeurNiveau[5, 4] = 5000; // Max Énergie
            TableauValeurNiveau[5, 5] = 500; //Pts de vie envoyer
            TableauValeurNiveau[5, 6] = 80; // Nb Énergie utilisé

            TableauValeurNiveau[6, 0] = 7;
            TableauValeurNiveau[6, 1] = 50000;
            TableauValeurNiveau[6, 2] = 10000; //cout bois
            TableauValeurNiveau[6, 3] = 5000; //cout or
            TableauValeurNiveau[6, 4] = 10000; // Max Énergie
            TableauValeurNiveau[6, 5] = 1000; //Pts de vie envoyer
            TableauValeurNiveau[6, 6] = 160; // Nb Énergie utilisé

            TableauValeurNiveau[7, 0] = 8;
            TableauValeurNiveau[7, 1] = 100000;
            TableauValeurNiveau[7, 2] = 25000; //cout bois
            TableauValeurNiveau[7, 3] = 12500; //cout or
            TableauValeurNiveau[7, 4] = 20000; // Max Énergie
            TableauValeurNiveau[7, 5] = 2000; //Pts de vie envoyer
            TableauValeurNiveau[7, 6] = 320; // Nb Énergie utilisé

            TableauValeurNiveau[8, 0] = 9;
            TableauValeurNiveau[8, 1] = 250000;
            TableauValeurNiveau[8, 2] = 50000; //cout bois
            TableauValeurNiveau[8, 3] = 25000; //cout or
            TableauValeurNiveau[8, 4] = 50000; // Max Énergie
            TableauValeurNiveau[8, 5] = 5000; //Pts de vie envoyer
            TableauValeurNiveau[8, 6] = 640; // Nb Énergie utilisé

            TableauValeurNiveau[9, 0] = 10;
            TableauValeurNiveau[9, 1] = 1000000;
            TableauValeurNiveau[9, 2] = 100000; //cout bois
            TableauValeurNiveau[9, 3] = 50000; //cout or
            TableauValeurNiveau[9, 4] = 100000; // Max Énergie
            TableauValeurNiveau[9, 5] = 10000; //Pts de vie envoyer
            TableauValeurNiveau[9, 6] = 1280; // Nb Énergie utilisé
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            NombreMaxPtsDeVie = (int)TableauValeurNiveau[0,1];
            NombrePtsDeVie = NombreMaxPtsDeVie;
            NombreMaxEnergie = (int)TableauValeurNiveau[0,4];
            NombrePtsEnergie = NombreMaxEnergie;
            NombrePtsDeVieEnvoyé = (int)TableauValeurNiveau[0,5];
            IntervalleEnvoieRecharge = 1f;
            NombreÉnergieUtilisé = (int)TableauValeurNiveau[0,6];
            Niveau = 1;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisEnvoie += tempsÉcoulé;
            if (TempsÉcouléDepuisEnvoie >= IntervalleEnvoieRecharge)
            {
                RéparerBatiments();
                TempsÉcouléDepuisEnvoie = 0;
            }
        }

        private void RéparerBatiments()
        {
            foreach (Batiment b in Game.Components.Where(x => x is Batiment))
            {
                if (b.NombrePtsDeVie < b.NombreMaxPtsDeVie && NombrePtsEnergie >= NombreÉnergieUtilisé)
                {
                    float distance = (float)Math.Abs(Math.Sqrt(Math.Pow((Position.X - b.Position.X), 2) + Math.Pow((Position.Z - b.Position.Z), 2)));
                    if (distance < 20)
                    {
                        b.NombrePtsDeVie += NombrePtsDeVieEnvoyé;
                        NombrePtsEnergie -= NombreÉnergieUtilisé;
                        if (b.NombrePtsDeVie > b.NombreMaxPtsDeVie)
                        {
                            b.NombrePtsDeVie = NombreMaxPtsDeVie;
                        }
                    }
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
                            NombreMaxEnergie = (int)TableauValeurNiveau[i + 1, 4];
                            NombrePtsEnergie = NombreMaxEnergie;
                            NombrePtsDeVieEnvoyé = (int)TableauValeurNiveau[i + 1, 5];
                            NombreÉnergieUtilisé = (int)TableauValeurNiveau[i + 1, 6];
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
