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
    public class Mur : Batiment
    {

        public Mur(Game game, string nomModele, float �chelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModele,�chelle,position,rotationInitiale)
        {
            // TODO: Construct any child components here
        }

        public override void Initialize()
        {
            base.Initialize();
            NombreMaxPtsDeVie = 1000;
            NombrePtsDeVie = NombreMaxPtsDeVie;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (NombrePtsDeVie <= 0)
            {
                Dispose();
            }
        }
    }
}
