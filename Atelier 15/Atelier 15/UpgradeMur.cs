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
    public class UpgradeMur : UpgradeIcon
    {
        InputManager GestionInput { get; set; }
        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        string Niveau { get; set; }
        SpriteFont ArialFont { get; set; }
        Mur MurSélectionné { get; set; }


        public UpgradeMur(Game game, Vector2 position, string locationTexture, Mur murSélectionner)
            : base(game,position, locationTexture)
        {
            MurSélectionné = murSélectionner;
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
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                GérerInput();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GestionSprite.Begin();
            DessinerInformation();
            GestionSprite.End();

            Game.GraphicsDevice.BlendState = BlendState.Opaque;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        private void DessinerInformation()
        {
            string niveauBatiment = "Niveau " + MurSélectionné.Niveau.ToString();
            string vieBatiment = "Vie "+MurSélectionné.NombrePtsDeVie.ToString() + "/" + MurSélectionné.NombreMaxPtsDeVie.ToString();
            string coutUpgrade = "Bois: " + MurSélectionné.TableauValeurNiveau[MurSélectionné.Niveau - 1, 2].ToString() + " / Or: " + MurSélectionné.TableauValeurNiveau[MurSélectionné.Niveau - 1, 3].ToString();

            GestionSprite.DrawString(ArialFont, niveauBatiment, new Vector2(Position.X - 16, Position.Y + 64), Color.White);
            GestionSprite.DrawString(ArialFont, vieBatiment, new Vector2(Position.X + 128, Position.Y), Color.Green);
            GestionSprite.DrawString(ArialFont, coutUpgrade, new Vector2(Position.X + 128, Position.Y + 32), Color.Blue);
        }

        private void GérerInput()
        {
            if (GestionInput.EstNouveauClicGauche() && TrouverSiIntersection())
            {
                MurSélectionné.MonterDeNiveau();
            }
        }
    }
}
