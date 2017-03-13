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
    public class UpgradeJoueurNombreRécolte : UpgradeIcon
    {
        InputManager GestionInput { get; set; }
        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        string Niveau { get; set; }
        SpriteFont ArialFont { get; set; }

        public UpgradeJoueurNombreRécolte(Game game, Vector2 position, string locationTexture)
            : base(game,position, locationTexture)
        {
            foreach (Joueur j in Game.Components.OfType<Joueur>())
            {
                Niveau = j.NiveauNombreRécolte.ToString();
            }
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
            DessinerNiveau();
            GestionSprite.End();

            Game.GraphicsDevice.BlendState = BlendState.Opaque;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        private void DessinerNiveau()
        {
            string niveauJoueur = "Récolte " + Niveau;
            GestionSprite.DrawString(ArialFont, niveauJoueur, new Vector2(Position.X-16, Position.Y + 64), Color.White);
        }

        private void GérerInput()
        {
            if (GestionInput.EstNouveauClicGauche() && TrouverSiIntersection())
            {
                FaireUpgrade();
            }
        }


        private void FaireUpgrade()
        {
            foreach (Joueur j in Game.Components.OfType<Joueur>())
            {
                if (j.NombreDOR >= 10)
                {
                    ++j.NombreCollectionRessource;
                    j.NombreDOR -= 10;
                    ++j.NiveauNombreRécolte;
                    Niveau = j.NiveauNombreRécolte.ToString();
                }
            }
        }
    }
}
