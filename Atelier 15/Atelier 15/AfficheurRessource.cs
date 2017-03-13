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

        public AfficheurRessource(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        public override void Initialize()
        {
            ArialFont = Game.Content.Load<SpriteFont>("Fonts/Arial20");
            GestionSprite = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            TrouverNombreRessource();
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprite.Begin();
            AfficherRessourceBois();
            AfficherRessourceOr();
            GestionSprite.End();
        }

        private void TrouverNombreRessource()
        {
            foreach(Joueur j in Game.Components.OfType<Joueur>())
            {
                NbOr = j.NombreDOR;
                NbBois = j.NombreDeBois;
            }
        }

        private void AfficherRessourceBois()
        {
            int offset = 32;

            Vector2 dimension = ArialFont.MeasureString(NbBois.ToString());
            Vector2 position = new Vector2(dimension.X + offset, dimension.Y + offset);

            GestionSprite.DrawString(ArialFont, NbBois.ToString(), position, Color.Red);
        }

        private void AfficherRessourceOr()
        {
            int offset = 32;

            Vector2 dimension = ArialFont.MeasureString(NbOr.ToString());
            Vector2 position = new Vector2(dimension.X + (2*offset), dimension.Y + offset);

            GestionSprite.DrawString(ArialFont, NbOr.ToString(), position, Color.Red);
        }
    }
}
