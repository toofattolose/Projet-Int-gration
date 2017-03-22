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
    public class EnemySpawner : Microsoft.Xna.Framework.GameComponent
    {
        public EnemySpawner(Game game)
            : base(game)
        {
            
        }

        public void CréerEnemy(int niveauEnemy)
        {
            Ennemis e = new Ennemis(Game, "player", 0.01f, new Vector3(256 / 2f + 2, 0, 256 / 2f + 2), Vector3.Zero, niveauEnemy);
            Game.Components.Add(e);
        }
    }
}
