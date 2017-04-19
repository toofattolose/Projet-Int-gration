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
    public class UpgradeJoueurDommage : UpgradeIcon
    {
        float IntervalleMAJ { get; set; }
        float Temps…coulÈDepuisMAJ { get; set; }
        int Niveau { get; set; }
        SpriteFont ArialFont { get; set; }
        float[,] tableauValeurNiveau = new float[10, 3];

        public UpgradeJoueurDommage(Game game,Vector2 position, string locationTexture)
            : base(game,position, locationTexture)
        {
            foreach(Joueur j in Game.Components.OfType<Joueur>())
            {
                Niveau = j.NiveauDommage;
            }

            tableauValeurNiveau[0, 0] = 1; //niveau
            tableauValeurNiveau[0, 1] = 125; //cout
            tableauValeurNiveau[0, 2] = 1; //nombre dommage

            tableauValeurNiveau[1, 0] = 2;
            tableauValeurNiveau[1, 1] = 375;
            tableauValeurNiveau[1, 2] = 2; //nombre dommage

            tableauValeurNiveau[2, 0] = 3;
            tableauValeurNiveau[2, 1] = 1125;
            tableauValeurNiveau[2, 2] = 4; //nombre dommage

            tableauValeurNiveau[3, 0] = 4;
            tableauValeurNiveau[3, 1] = 3375;
            tableauValeurNiveau[3, 2] = 8; //nombre dommage

            tableauValeurNiveau[4, 0] = 5;
            tableauValeurNiveau[4, 1] = 10125;
            tableauValeurNiveau[4, 2] = 16; //nombre dommage

            tableauValeurNiveau[5, 0] = 6;
            tableauValeurNiveau[5, 1] = 30375;
            tableauValeurNiveau[5, 2] = 32; //nombre dommage

            tableauValeurNiveau[6, 0] = 7;
            tableauValeurNiveau[6, 1] = 91125;
            tableauValeurNiveau[6, 2] = 64; //nombre dommage

            tableauValeurNiveau[7, 0] = 8;
            tableauValeurNiveau[7, 1] = 273375;
            tableauValeurNiveau[7, 2] = 128; //nombre dommage

            tableauValeurNiveau[8, 0] = 9;
            tableauValeurNiveau[8, 1] = 820125;
            tableauValeurNiveau[8, 2] = 256; //nombre dommage

            tableauValeurNiveau[9, 0] = 10;
            tableauValeurNiveau[9, 1] = 0;
            tableauValeurNiveau[9, 2] = 512; //nombre dommage
        }

        public override void Initialize()
        {
            base.Initialize();
            IntervalleMAJ = 1 / 60f;
            ArialFont = Game.Content.Load<SpriteFont>("Fonts/Arial20");
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
        }

        public override void Update(GameTime gameTime)
        {
            float temps…coulÈ = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps…coulÈDepuisMAJ += temps…coulÈ;
            if (Temps…coulÈDepuisMAJ >= IntervalleMAJ)
            {
                GÈrerInput();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GestionSprite.Begin();
            DessinerNiveau();
            GestionSprite.End();
            //lol
            Game.GraphicsDevice.BlendState = BlendState.Opaque;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        private void DessinerNiveau()
        {
            string niveauJoueur = "Dommage " + Niveau.ToString();
            string coutOr = "Or: " + tableauValeurNiveau[Niveau - 1, 1].ToString();
            GestionSprite.DrawString(ArialFont, niveauJoueur, new Vector2(Position.X-16, Position.Y + 64), Color.White);
            GestionSprite.DrawString(ArialFont, coutOr, new Vector2(Position.X, Position.Y - 32), Color.Yellow);
        }

        private void GÈrerInput()
        {
            if (GestionInput.EstNouveauClicGauche() && TrouverSiIntersection())
            {
                FaireUpgrade();
            }
        }


        private void FaireUpgrade()
        {
            bool estUpgrader = false;
            foreach (Joueur j in Game.Components.OfType<Joueur>())
            {
                for (int i = 0; i < tableauValeurNiveau.GetLength(0); i++)
                {
                    if (Niveau == tableauValeurNiveau[i, 0] && Niveau != 10 && !estUpgrader)
                    {
                        if (j.NombreDOR >= tableauValeurNiveau[i, 1])
                        {
                            SoundAchat.Play();
                            j.Dommage = (int)tableauValeurNiveau[i + 1, 2];
                            j.NombreDOR -= (int)tableauValeurNiveau[i, 1];
                            ++j.NiveauDommage;
                            Niveau = j.NiveauDommage;
                            estUpgrader = true;
                        }
                    }
                }
            }
        }
    }
}
