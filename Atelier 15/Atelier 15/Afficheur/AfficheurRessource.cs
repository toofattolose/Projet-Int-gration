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
        SpriteFont ArialFont { get; set; }
        SpriteBatch GestionSprite;
        float IntervalleMAJ { get; set; }
        float Temps�coul�DepuisMAJ { get; set; }

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
            float temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps�coul�DepuisMAJ += temps�coul�;
            if (Temps�coul�DepuisMAJ > IntervalleMAJ)
            {
                TrouverNombreRessource();
                Temps�coul�DepuisMAJ = 0;
            }         
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprite.Begin();
            AfficherRessourceBois(NbBois.ToString());
            AfficherRessourceOr(NbOr.ToString());
            GestionSprite.End();
            Game.GraphicsDevice.BlendState = BlendState.Opaque;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        private void TrouverNombreRessource()
        {
            foreach(Joueur j in Game.Components.OfType<Joueur>())
            {
                NbOr = j.NombreDOR;
                NbBois = j.NombreDeBois;
            }
        }

        private void AfficherRessourceBois(string ressource)
        {
            int offset = 32;

            Vector2 dimension = ArialFont.MeasureString(ressource);
            Vector2 position = new Vector2(dimension.X + offset, dimension.Y);

            GestionSprite.DrawString(ArialFont, ressource, position, Color.White);
        }

        private void AfficherRessourceOr(string ressource)
        {
            int offset = 32;

            Vector2 dimension = ArialFont.MeasureString(ressource);
            Vector2 position = new Vector2(dimension.X + (10*offset), dimension.Y);

            GestionSprite.DrawString(ArialFont, ressource, position, Color.White);
        }
    }
}