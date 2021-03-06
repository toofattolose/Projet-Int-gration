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
    public class AfficheurCollectionRessource : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Vector3 Position3D { get; set; }
        Vector2 Position { get; set; }
        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        float TempsÉcouléPourDisposition { get; set; }
        float TempsAvantMort { get; set; }
        Color Couleur { get; set; }
        SpriteFont Arial { get; set; }
        SpriteBatch GestionSprite { get; set; }
        int NbCollectionRessource { get; set; }
        Caméra3rdPerson Camera { get; set; }
        Vector2 Déplacement { get; set; }
        Vector3 PositionTemp { get; set; }

        public AfficheurCollectionRessource(Game game, Vector3 position, Color couleur, int nbRessource)
            : base(game)
        {
            Position3D = position;
            Couleur = couleur;
            NbCollectionRessource = nbRessource;
        }

        public override void Initialize()
        {
            // TODO: Add your initialization code here
            Déplacement = new Vector2(0, -1.5f);
            IntervalleMAJ = 1 / 60f;
            TempsAvantMort = 0.5f;
            Arial = Game.Content.Load<SpriteFont>("Fonts/Arial20");
            Camera = Game.Services.GetService(typeof(Caméra)) as Caméra3rdPerson;
            GestionSprite = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            PositionTemp = Game.GraphicsDevice.Viewport.Project(Position3D, Camera.Projection, Camera.Vue, Matrix.Identity);
            Position = new Vector2(PositionTemp.X, PositionTemp.Y);
            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            TempsÉcouléPourDisposition += tempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                FaireMouvement();
                TempsÉcouléDepuisMAJ = 0;
            }
            if (TempsÉcouléPourDisposition >= TempsAvantMort)
            {
                Dispose();
            }
        }

        private void FaireMouvement()
        {
            Position += Déplacement;
            
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprite.Begin();
            GestionSprite.DrawString(Arial, "+" + NbCollectionRessource.ToString(), Position, Couleur);
            GestionSprite.End();
            Game.GraphicsDevice.BlendState = BlendState.Opaque;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }
    }
}
