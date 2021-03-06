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
        float TempsÉcouléDepuisMAJ { get; set; }
        float TempsPhaseNuit { get; set; }
        float TempsSpawnEnnemis { get; set; }
        string État { get; set; }
        int i { get; set; }
        Game Jeu { get; set; }
        InputManager GestionInput { get; set; }
        int NumVague { get; set; }

        public ControlePhaseDeJeu(Game game, float tempsPhaseJour, float tempsPhaseNuit)
            : base(game)
        {
            Jeu = game;
            TempsPhaseJour = tempsPhaseJour;
            TempsPhaseNuit = tempsPhaseNuit;
            État = "jour";
            i = 0;
            NumVague = 1;
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
        }

        public override void Update(GameTime gameTime)
        {
            switch (État)
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
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= TempsPhaseJour)
            {
                TempsÉcouléDepuisMAJ = 0;
                État = "nuit";
            }
        }

        private void FaireMAJNuit(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            TempsSpawnEnnemis += tempsÉcoulé;
            if (TempsSpawnEnnemis >= 2)
            //if(GestionInput.EstEnfoncée(Keys.P))
            {
                ++i;
                TempsSpawnEnnemis = 0;
            }
            if (TempsÉcouléDepuisMAJ >= TempsPhaseNuit)
            {
                TempsÉcouléDepuisMAJ = 0;
                NumVague++;
                État = "jour";
            }
        }
    }
}
