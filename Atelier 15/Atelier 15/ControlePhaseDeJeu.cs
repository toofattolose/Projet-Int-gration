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
        float TempsSpawnEnnemis { get; set; }
        string �tat { get; set; }
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
            �tat = "jour";
            i = 0;
            NumVague = 1;
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
        }

        public override void Update(GameTime gameTime)
        {
            switch (�tat)
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
            TempsSpawnEnnemis += temps�coul�;
            if (TempsSpawnEnnemis >= 2)
            //if(GestionInput.EstEnfonc�e(Keys.P))
            {
                Game.Components.Add(new Ennemis(Jeu, "player", 0.01f, new Vector3(256 / 2f, 0, 256 / 2f), Vector3.Zero, NumVague));
                ++i;
                TempsSpawnEnnemis = 0;
            }
            if (Temps�coul�DepuisMAJ >= TempsPhaseNuit)
            {
                Temps�coul�DepuisMAJ = 0;
                NumVague++;
                �tat = "jour";
            }
        }
    }
}
