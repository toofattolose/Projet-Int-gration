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
    public class Batiment : Model3D
    {
        public int NombrePtsDeVie { get; set; }
        public int NombreMaxPtsDeVie { get; set; }
        public int Niveau { get; set; }
        protected SoundEffect SoundUpgrade { get; set; }

        public Batiment(Game game, string nomModele, float échelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModele,échelle,position,rotationInitiale)
        {
            // TODO: Construct any child components here
        }

        public override void Initialize()
        {
            base.Initialize();
            SoundUpgrade = Game.Content.Load<SoundEffect>("SoundEffects/wololo");
            Niveau = 1;
        }
    }
}
