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
    public class UpgradeIcon : DrawableGameComponent
    {
        protected InputManager GestionInput { get; set; }
        protected SpriteBatch GestionSprite { get; set; }
        protected Texture2D SpriteUpgrade { get; set; }
        protected Vector2 Position { get; set; }
        string LocationTexture { get; set; }
        

        public UpgradeIcon(Game game, Vector2 position, string locationTexture)
            : base(game)
        {
            Position = position;
            LocationTexture = locationTexture;
        }

        public override void Initialize()
        {
            // TODO: Add your initialization code here
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GestionSprite = new SpriteBatch(Game.GraphicsDevice);
            SpriteUpgrade = Game.Content.Load<Texture2D>(LocationTexture);
            base.Initialize();
        }

        

        public override void Draw(GameTime gameTime)
        {
            GestionSprite.Begin();
            DessinerIcon();
            GestionSprite.End();

            Game.GraphicsDevice.BlendState = BlendState.Opaque;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        private void DessinerIcon()
        {
            GestionSprite.Draw(SpriteUpgrade, Position, Color.White);
        }

        protected bool TrouverSiIntersection()
        {
            Point pointPosSouris = GestionInput.GetPositionSouris();
            Vector2 positionSouris = new Vector2(pointPosSouris.X, pointPosSouris.Y);
            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 64; j++)
                {                 
                    if ((positionSouris.X == Position.X + i) && positionSouris.Y == Position.Y + j)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
