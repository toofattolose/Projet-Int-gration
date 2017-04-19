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
    public class UpgradeJoueurFiringRate : UpgradeIcon
    {
        float[,] tableauValeurNiveau = new float[5, 3];
        float IntervalleMAJ { get; set; }
        float Temps…coulÈDepuisMAJ { get; set; }
        int Niveau { get; set; }
        SpriteFont ArialFont { get; set; }

        public UpgradeJoueurFiringRate(Game game, Vector2 position, string locationTexture)
            : base(game,position, locationTexture)
        {
            foreach (Joueur j in Game.Components.OfType<Joueur>())
            {
                Niveau = j.NiveauFiringRate;
            }
            tableauValeurNiveau[0, 0] = 1;
            tableauValeurNiveau[0, 1] = 1f;
            tableauValeurNiveau[0, 2] = 1125;

            tableauValeurNiveau[1, 0] = 2;
            tableauValeurNiveau[1, 1] = 1/2f;
            tableauValeurNiveau[1, 2] = 10125;

            tableauValeurNiveau[2, 0] = 3;
            tableauValeurNiveau[2, 1] = 1/3f;
            tableauValeurNiveau[2, 2] = 91125;

            tableauValeurNiveau[3, 0] = 4;
            tableauValeurNiveau[3, 1] = 1/5f;
            tableauValeurNiveau[3, 2] = 820125;

            tableauValeurNiveau[4, 0] = 5;
            tableauValeurNiveau[4, 1] = 1/10f;
            tableauValeurNiveau[4, 2] = 10;
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

            Game.GraphicsDevice.BlendState = BlendState.Opaque;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        private void DessinerNiveau()
        {
            string niveauJoueur = "Cadence " + Niveau.ToString();
            string coutOr = "Or: " + tableauValeurNiveau[Niveau - 1, 2];
            GestionSprite.DrawString(ArialFont, niveauJoueur, new Vector2(Position.X-16 , Position.Y + 64), Color.White);
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
                if (j.NombreDOR >= 10)
                {
                    for (int i = 0; i < tableauValeurNiveau.GetLength(0); i++)
                    {
                        if (tableauValeurNiveau[i,0] == j.NiveauFiringRate && j.NiveauFiringRate != 5 && !estUpgrader)
                        {
                            SoundAchat.Play();
                            j.FiringRate = tableauValeurNiveau[i + 1,1];
                            j.NombreDOR -= (int)tableauValeurNiveau[i,2];
                            ++j.NiveauFiringRate;
                            Niveau = j.NiveauFiringRate;
                            estUpgrader = true;
                        } 
                    }                  
                }
            }
        }
    }
}
