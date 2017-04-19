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
    public class AfficheurRessource : Microsoft.Xna.Framework.DrawableGameComponent
    {
        int NbBois { get; set; }
        int NbOr { get; set; }
        int NbPts { get; set; }
        SpriteFont ArialFont { get; set; }
        SpriteBatch GestionSprite;
        float IntervalleMAJ { get; set; }
        float Temps…coulÈDepuisMAJ { get; set; }

        public AfficheurRessource(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        public override void Initialize()
        {
            ArialFont = Game.Content.Load<SpriteFont>("Fonts/Arial20");
            GestionSprite = new SpriteBatch(Game.GraphicsDevice);
            IntervalleMAJ = 1 / 60f;
        }

        public override void Update(GameTime gameTime)
        {
            float temps…coulÈ = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps…coulÈDepuisMAJ += temps…coulÈ;
            if (Temps…coulÈDepuisMAJ > IntervalleMAJ)
            {
                TrouverNombreRessource();
                Temps…coulÈDepuisMAJ = 0;
            }         
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprite.Begin();
            AfficherRessourceBois("Bois: "+NbBois.ToString());
            AfficherRessourceOr("Or: "+NbOr.ToString());
            GestionSprite.End();
            Game.GraphicsDevice.BlendState = BlendState.Opaque;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        private void TrouverNombreRessource()
        {
            try
            {
                foreach (Joueur j in Game.Components.OfType<Joueur>())
                {
                    NbOr = j.NombreDOR;
                    NbBois = j.NombreDeBois;
                    NbPts = j.NombrePtsKill;
                }
            }
            catch (Exception) { }
        }

        private void AfficherRessourceBois(string ressource)
        {
            int offset = 32;

            Vector2 dimension = ArialFont.MeasureString(ressource);
            Vector2 position = new Vector2(offset, dimension.Y);

            GestionSprite.DrawString(ArialFont, ressource, position, Color.Red);
        }

        private void AfficherRessourceOr(string ressource)
        {
            int offset = 32;

            Vector2 dimension = ArialFont.MeasureString(ressource);
            Vector2 position = new Vector2(offset, dimension.Y + offset);

            GestionSprite.DrawString(ArialFont, ressource, position, Color.Red);
        }

        private void AfficherRessourcePoints (string ressource)
        {
            int offset = 32;

            Vector2 dimension = ArialFont.MeasureString(ressource);
            Vector2 position = new Vector2(offset, dimension.Y + offset * 2);

            GestionSprite.DrawString(ArialFont, ressource, position, Color.Red);
        }
    }
}
