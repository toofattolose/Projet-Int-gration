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
    public class UpgradeMur : UpgradeIcon
    {
        InputManager GestionInput { get; set; }
        float IntervalleMAJ { get; set; }
        float Temps�coul�DepuisMAJ { get; set; }
        string Niveau { get; set; }
        SpriteFont ArialFont { get; set; }
        Mur MurS�lectionn� { get; set; }


        public UpgradeMur(Game game, Vector2 position, string locationTexture, Mur murS�lectionner)
            : base(game, position, locationTexture)
        {
            MurS�lectionn� = murS�lectionner;
        }

        public override void Initialize()
        {
            base.Initialize();
            IntervalleMAJ = 1 / 60f;
            ArialFont = Game.Content.Load<SpriteFont>("Fonts/Arial20");
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

        private void G�rerInput()
        {
            if (GestionInput.EstNouveauClicGauche() && TrouverSiIntersection())
            {
                MurS�lectionn�.MonterDeNiveau();
            }
        }
    }
}
