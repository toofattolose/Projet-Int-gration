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
        float Temps�coul�DepuisMAJ { get; set; }
        float Temps�coul�PourDisposition { get; set; }
        float TempsAvantMort { get; set; }
        Color Couleur { get; set; }
        SpriteFont Arial { get; set; }
        SpriteBatch GestionSprite { get; set; }
        int NbCollectionRessource { get; set; }
        Cam�ra3rdPerson Camera { get; set; }
        Vector2 D�placement { get; set; }
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
            D�placement = new Vector2(0, -1.5f);
            IntervalleMAJ = 1 / 60f;
            TempsAvantMort = 0.5f;
            Arial = Game.Content.Load<SpriteFont>("Fonts/Arial20");
            Camera = Game.Services.GetService(typeof(Cam�ra)) as Cam�ra3rdPerson;
            GestionSprite = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            PositionTemp = Game.GraphicsDevice.Viewport.Project(Position3D, Camera.Projection, Camera.Vue, Matrix.Identity);
            Position = new Vector2(PositionTemp.X, PositionTemp.Y);
            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            float temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps�coul�DepuisMAJ += temps�coul�;
            Temps�coul�PourDisposition += temps�coul�;
            if (Temps�coul�DepuisMAJ >= IntervalleMAJ)
            {
                FaireMouvement();
                Temps�coul�DepuisMAJ = 0;
            }
            if (Temps�coul�PourDisposition >= TempsAvantMort)
            {
                Dispose();
            }
        }

        private void FaireMouvement()
        {
            Position += D�placement;
            
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
