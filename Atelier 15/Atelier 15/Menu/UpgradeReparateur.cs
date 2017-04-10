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
    public class UpgradeReparateur : UpgradeIcon
    {
        float IntervalleMAJ { get; set; }
        float Temps…coulÈDepuisMAJ { get; set; }
        string Niveau { get; set; }
        SpriteFont ArialFont { get; set; }
        Reparateur ReparateurSelectionner { get; set; }


        public UpgradeReparateur(Game game, Vector2 position, string locationTexture, Reparateur reparateurSelectionner)
            : base(game,position, locationTexture)
        {
            ReparateurSelectionner = reparateurSelectionner;
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
            DessinerInformation();
            GestionSprite.End();

            Game.GraphicsDevice.BlendState = BlendState.Opaque;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        private void DessinerInformation()
        {
            string niveauBatiment = "Niveau " + ReparateurSelectionner.Niveau.ToString();
            string vieBatiment = "Vie " + ReparateurSelectionner.NombrePtsDeVie.ToString() + "/" + ReparateurSelectionner.NombreMaxPtsDeVie.ToString();
            string energieBatiment = "…nergie: " + ReparateurSelectionner.NombrePtsEnergie.ToString() + "/" + ReparateurSelectionner.NombreMaxEnergie.ToString();
            string coutUpgrade = "Bois: " + ReparateurSelectionner.TableauValeurNiveau[ReparateurSelectionner.Niveau - 1, 2].ToString() + " / Or: " + ReparateurSelectionner.TableauValeurNiveau[ReparateurSelectionner.Niveau - 1, 3].ToString();

            GestionSprite.DrawString(ArialFont, niveauBatiment, new Vector2(Position.X - 16, Position.Y + 64), Color.White);
            GestionSprite.DrawString(ArialFont, vieBatiment, new Vector2(Position.X + 128, Position.Y), Color.Green);
            GestionSprite.DrawString(ArialFont, energieBatiment, new Vector2(Position.X + 256, Position.Y), Color.Green);
            GestionSprite.DrawString(ArialFont, coutUpgrade, new Vector2(Position.X + 128, Position.Y + 32), Color.Blue);
        }

        private void GÈrerInput()
        {
            if (GestionInput.EstNouveauClicGauche() && TrouverSiIntersection())
            {
                ReparateurSelectionner.MonterDeNiveau();
            }
        }
    }
}
