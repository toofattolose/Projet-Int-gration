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
    public class Arbre : Model3D
    {
        public Arbre(Game game, string nomModele, float échelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModele, échelle, position, rotationInitiale)
        {

        }
    }
}
