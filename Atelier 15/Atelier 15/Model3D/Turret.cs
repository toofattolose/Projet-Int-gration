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
    public class Turret : BatimentRechargable
    {
        public float[,] TableauValeurNiveau = new float[10, 8];
        Ennemis Cible { get; set; }
        float Angle { get; set; }
        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        SoundEffect SoundShooting { get; set; }

        //attaque
        float IntervalleDeTir { get; set; }
        float TempsDepuisDernierTir { get; set; }
        int CoûtDeTir { get; set; }
        int DistanceDeTir { get; set; }
        int Dmg { get; set; }
        Vector3 Direction { get; set; }
        bool EnnemiTrouvé { get; set; }
        

        public Turret(Game game, string nomModele, float échelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModele,échelle,position,rotationInitiale)
        {
            // TODO: Construct any child components here
            TableauValeurNiveau[0, 0] = 1; //niveau
            TableauValeurNiveau[0, 1] = 100; //nb vie
            TableauValeurNiveau[0, 2] = 0; //cout bois
            TableauValeurNiveau[0, 3] = 0; //cout or
            TableauValeurNiveau[0, 4] = 1; //Damage
            TableauValeurNiveau[0, 5] = 1f; //intervalle de tir
            TableauValeurNiveau[0, 6] = 2; //Cout énergie
            TableauValeurNiveau[0, 7] = 100; //Énergie max

            TableauValeurNiveau[1, 0] = 2;
            TableauValeurNiveau[1, 1] = 150;
            TableauValeurNiveau[1, 2] = 0; //cout bois
            TableauValeurNiveau[1, 3] = 0; //cout or
            TableauValeurNiveau[1, 4] = 2; //Damage
            TableauValeurNiveau[1, 5] = 1f; //intervalle de tir
            TableauValeurNiveau[1, 6] = 2; //Cout énergie
            TableauValeurNiveau[1, 7] = 200; //Énergie max

            TableauValeurNiveau[2, 0] = 3;
            TableauValeurNiveau[2, 1] = 200;
            TableauValeurNiveau[2, 2] = 500; //cout bois
            TableauValeurNiveau[2, 3] = 250; //cout or
            TableauValeurNiveau[2, 4] = 4; //Damage
            TableauValeurNiveau[2, 5] = 1f; //intervalle de tir
            TableauValeurNiveau[2, 6] = 3; //Cout énergie
            TableauValeurNiveau[2, 7] = 300; //Énergie max

            TableauValeurNiveau[3, 0] = 4;
            TableauValeurNiveau[3, 1] = 250;
            TableauValeurNiveau[3, 2] = 1000; //cout bois
            TableauValeurNiveau[3, 3] = 500; //cout or
            TableauValeurNiveau[3, 4] = 8; //Damage
            TableauValeurNiveau[3, 5] = 1f; //intervalle de tir
            TableauValeurNiveau[3, 6] = 4; //Cout énergie
            TableauValeurNiveau[3, 7] = 400; //Énergie max

            TableauValeurNiveau[4, 0] = 5;
            TableauValeurNiveau[4, 1] = 300;
            TableauValeurNiveau[4, 2] = 2500; //cout bois
            TableauValeurNiveau[4, 3] = 1250; //cout or
            TableauValeurNiveau[4, 4] = 16; //Damage
            TableauValeurNiveau[4, 5] = 0.5f; //intervalle de tir
            TableauValeurNiveau[4, 6] = 5; //Cout énergie
            TableauValeurNiveau[4, 7] = 500; //Énergie max

            TableauValeurNiveau[5, 0] = 6;
            TableauValeurNiveau[5, 1] = 500;
            TableauValeurNiveau[5, 2] = 5000; //cout bois
            TableauValeurNiveau[5, 3] = 2500; //cout or
            TableauValeurNiveau[5, 4] = 32; //Damage
            TableauValeurNiveau[5, 5] = 0.5f; //intervalle de tir
            TableauValeurNiveau[5, 6] = 6; //Cout énergie
            TableauValeurNiveau[5, 7] = 600; //Énergie max

            TableauValeurNiveau[6, 0] = 7;
            TableauValeurNiveau[6, 1] = 750;
            TableauValeurNiveau[6, 2] = 10000; //cout bois
            TableauValeurNiveau[6, 3] = 5000; //cout or
            TableauValeurNiveau[6, 4] = 64; //Damage
            TableauValeurNiveau[6, 5] = 0.5f; //intervalle de tir
            TableauValeurNiveau[6, 6] = 7; //Cout énergie
            TableauValeurNiveau[6, 7] = 700; //Énergie max

            TableauValeurNiveau[7, 0] = 8;
            TableauValeurNiveau[7, 1] = 1000;
            TableauValeurNiveau[7, 2] = 25000; //cout bois
            TableauValeurNiveau[7, 3] = 12500; //cout or
            TableauValeurNiveau[7, 4] = 128; //Damage
            TableauValeurNiveau[7, 5] = 0.5f; //intervalle de tir
            TableauValeurNiveau[7, 6] = 8; //Cout énergie
            TableauValeurNiveau[7, 7] = 800; //Énergie max

            TableauValeurNiveau[8, 0] = 9;
            TableauValeurNiveau[8, 1] = 1500;
            TableauValeurNiveau[8, 2] = 50000; //cout bois
            TableauValeurNiveau[8, 3] = 25000; //cout or
            TableauValeurNiveau[8, 4] = 256; //Damage
            TableauValeurNiveau[8, 5] = 0.5f; //intervalle de tir
            TableauValeurNiveau[8, 6] = 9; //Cout énergie
            TableauValeurNiveau[8, 7] = 900; //Énergie max

            TableauValeurNiveau[9, 0] = 10;
            TableauValeurNiveau[9, 1] = 2000;
            TableauValeurNiveau[9, 2] = 100000; //cout bois
            TableauValeurNiveau[9, 3] = 50000; //cout or
            TableauValeurNiveau[9, 4] = 512; //Damage
            TableauValeurNiveau[9, 5] = 0.5f; //intervalle de tir
            TableauValeurNiveau[9, 6] = 10; //Cout énergie
            TableauValeurNiveau[9, 7] = 1000; //Énergie max
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            SoundShooting = Game.Content.Load<SoundEffect>("SoundEffects/shooting");
            Angle = 0;
            DistanceDeTir = 20;
            NombreMaxPtsDeVie = (int)TableauValeurNiveau[0,1];
            NombrePtsDeVie = NombreMaxPtsDeVie;
            NombreMaxEnergie = (int)TableauValeurNiveau[0,7];
            NombrePtsEnergie = NombreMaxEnergie;
            Niveau = (int)TableauValeurNiveau[0,0];
            Dmg = (int)TableauValeurNiveau[0, 4];
            IntervalleDeTir = TableauValeurNiveau[0, 5];
            CoûtDeTir = (int)TableauValeurNiveau[0, 6];
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsDepuisDernierTir += tempsÉcoulé;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            base.Update(gameTime);
            
            
            if (TempsDepuisDernierTir >= IntervalleDeTir)
            {
                if (NombrePtsEnergie >= CoûtDeTir)
                {
                    GérerTir();
                }               
                TempsDepuisDernierTir = 0;
            }
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                GérerChoixCible();
                CalculerMonde();
                TempsÉcouléDepuisMAJ = 0;
            }
        }

        private void GérerChoixCible()
        {
            try
            {
                EnnemiTrouvé = false;
                foreach (Ennemis e in Game.Components.OfType<Ennemis>())
                {
                    float distance = (float)Math.Abs(Math.Sqrt(Math.Pow(e.Position.X - Position.X, 2) + Math.Pow(e.Position.Y - Position.Y, 2) + Math.Pow(e.Position.Z - Position.Z, 2)));
                    if (distance <= DistanceDeTir && Cible == null)
                    {
                        Cible = e;   
                    }
                }
                foreach(Ennemis e in Game.Components.OfType<Ennemis>())
                {
                    if (e == Cible)
                    {
                        float distance = (float)Math.Abs(Math.Sqrt(Math.Pow(e.Position.X - Position.X, 2) + Math.Pow(e.Position.Y - Position.Y, 2) + Math.Pow(e.Position.Z - Position.Z, 2)));
                        if (distance <= DistanceDeTir)
                        {
                            Direction = new Vector3(e.Position.X - Position.X, 0, e.Position.Z - Position.Z);
                            Direction = Vector3.Normalize(Direction);
                            Vector3 directionBase = Vector3.UnitX;
                            directionBase.Normalize();
                            double cosAngle = Vector3.Dot(Direction, directionBase);
                            if (e.Position.Z > Position.Z)
                            {
                                Angle = -(float)Math.Acos(cosAngle);
                            }
                            else
                            {
                                Angle = (float)Math.Acos(cosAngle);
                            }
                            Rotation = new Vector3(0, Angle, 0);
                            EnnemiTrouvé = true;
                        }         
                    }
                }
                if (!EnnemiTrouvé)
                {
                    Cible = null;
                }         
            }
            catch (Exception) { }
        }

        private void GérerTir()
        {
            if (Cible != null)
            {
                SoundShooting.Play();
                BalleJoueur balle = new BalleJoueur(Game, "bullet", 0.015f, Position + new Vector3(0, 1, 0), Rotation, Dmg, Direction, 1 / 60f, 2f);
                Game.Components.Add(balle);
                NombrePtsEnergie -= CoûtDeTir;
            }
        }

        protected override void CalculerMonde()
        {
            Monde = Matrix.Identity;
            Monde *= Matrix.CreateScale(Échelle);
            Monde *= Matrix.CreateFromAxisAngle(Vector3.Up, Angle);
            Monde *= Matrix.CreateTranslation(Position);
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
                            NombreMaxEnergie = (int)TableauValeurNiveau[i + 1, 7];
                            NombrePtsEnergie = NombreMaxEnergie;
                            Dmg = (int)TableauValeurNiveau[i + 1, 4];
                            IntervalleDeTir = TableauValeurNiveau[i+1, 5];
                            CoûtDeTir = (int)TableauValeurNiveau[i+1, 6];
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
