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

        public MenuUpgradeJoueur(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }


        public override void Initialize()
        {
            GestionSprite = new SpriteBatch(Game.GraphicsDevice);
            SpriteMenu = Game.Content.Load<Texture2D>("Sprites/proj_finale_background");
            //Va créer les upgrades du joueur (components)
            //Game.Components.Add(new UpgradeJoueurDommage(Game, new Vector2(32,300 + 36),"Sprites/spr_upgrade_icon_1"));
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
            GestionSprite.Draw(SpriteMenu, new Vector2(0, 300), Color.White);
        }
    }
}
