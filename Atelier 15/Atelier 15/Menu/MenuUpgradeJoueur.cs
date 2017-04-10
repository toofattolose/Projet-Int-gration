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
    public class MenuUpgradeJoueur : DrawableGameComponent
    {
        SpriteBatch GestionSprite { get; set; }
        Texture2D SpriteMenu { get; set; }
        Vector2 PositionDans…cran { get; set; }

        public MenuUpgradeJoueur(Game game, Vector2 positionDans…cran)
            : base(game)
        {
            // TODO: Construct any child components here
            PositionDans…cran = positionDans…cran;
        }


        public override void Initialize()
        {
            GestionSprite = new SpriteBatch(Game.GraphicsDevice);
            SpriteMenu = Game.Content.Load<Texture2D>("Sprites/proj_finale_background");
            //Va crÈer les upgrades du joueur (components)
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprite.Begin();
            DessinerMenu();
            GestionSprite.End();

            Game.GraphicsDevice.BlendState = BlendState.Opaque;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        private void DessinerMenu()
        {
            GestionSprite.Draw(SpriteMenu, PositionDans…cran, Color.White);
        }
    }
}
