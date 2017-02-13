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
    public class Roche : Model3D, IClicDroit
    {
        GridDeJeu Grid { get; set; }

        public Roche(Game game, string nomModele, float échelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModele, échelle, position, rotationInitiale)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            Grid = Game.Services.GetService(typeof(GridDeJeu)) as GridDeJeu;
        }

        public void EstCliquéDroit()
        {
            Vector2 positionDansGrid = new Vector2((float)Math.Floor(Position.X / Grid.Delta.X), (float)Math.Floor(Position.Z / Grid.Delta.Y));
            Grid.TableauGrid[(int)positionDansGrid.X, (int)positionDansGrid.Y] = true;
            Dispose();
        }
    }
}
