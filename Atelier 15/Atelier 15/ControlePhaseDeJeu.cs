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
    public class ControlePhaseDeJeu : Microsoft.Xna.Framework.GameComponent
    {
        float TempsPhaseJour { get; set; }
        float Temps�coul�DepuisMAJ { get; set; }
        float TempsPhaseNuit { get; set; }
        string �tat { get; set; }

        public ControlePhaseDeJeu(Game game, float tempsPhaseJour, float tempsPhaseNuit)
            : base(game)
        {
            TempsPhaseJour = tempsPhaseJour;
            TempsPhaseNuit = TempsPhaseNuit;
            �tat = "jour";
        }



        public override void Update(GameTime gameTime)
        {
            switch(�tat)
            {
                case "jour":
                    FaireMAJJour(gameTime);
                    break;
                case "nuit":
                    FaireMAJNuit(gameTime);
                    break;
            }
        }



        private void FaireMAJJour(GameTime gameTime)
        {
            float temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps�coul�DepuisMAJ += temps�coul�;
            if (Temps�coul�DepuisMAJ >= TempsPhaseJour)
            {
                Temps�coul�DepuisMAJ = 0;
                �tat = "nuit";
            }
        }

        private void FaireMAJNuit(GameTime gameTime)
        {
            float temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps�coul�DepuisMAJ += temps�coul�;
            if (Temps�coul�DepuisMAJ >= TempsPhaseNuit)
            {
                Temps�coul�DepuisMAJ = 0;
                �tat = "jour";
            }
        }
    }
}
