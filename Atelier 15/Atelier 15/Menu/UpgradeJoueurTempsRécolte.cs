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
    public class UpgradeJoueurTempsRécolte : UpgradeIcon
    {
        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        int Niveau { get; set; }
        SpriteFont ArialFont { get; set; }
        float[,] tableauValeurNiveau = new float[6, 3];

        public UpgradeJoueurTempsRécolte(Game game, Vector2 position, string locationTexture)
            : base(game,position, locationTexture)
        {
            foreach (Joueur j in Game.Components.OfType<Joueur>())
            {
                Niveau = j.NiveauTempsRécolte;
            }

            tableauValeurNiveau[0, 0] = 1;
            tableauValeurNiveau[0, 1] = 1f;
            tableauValeurNiveau[0, 2] = 10;
            tableauValeurNiveau[1, 0] = 2;
            tableauValeurNiveau[1, 1] = 1 / 2f;
            tableauValeurNiveau[1, 2] = 10;
            tableauValeurNiveau[2, 0] = 3;
            tableauValeurNiveau[2, 1] = 1 / 3f;
            tableauValeurNiveau[2, 2] = 10;
            tableauValeurNiveau[3, 0] = 4;
            tableauValeurNiveau[3, 1] = 1 / 5f;
            tableauValeurNiveau[3, 2] = 10;
            tableauValeurNiveau[4, 0] = 5;
            tableauValeurNiveau[4, 1] = 1 / 10f;
            tableauValeurNiveau[4, 2] = 10;
            tableauValeurNiveau[5, 0] = 6;
            tableauValeurNiveau[5, 1] = 0;
            tableauValeurNiveau[5, 2] = 0;
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
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                GérerInput();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GestionSprite.Begin();
            DessinerNiveau();
            GestionSprite.End();

            Game.GraphicsDevice.BlendState = BlendState.Opaque;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        private void DessinerNiveau()
        {
            string niveauJoueur = "Collection " + Niveau.ToString();
            string coutOr = "Or: " + tableauValeurNiveau[Niveau - 1 , 2];
            GestionSprite.DrawString(ArialFont, niveauJoueur, new Vector2(Position.X - 16, Position.Y + 64), Color.White);
            GestionSprite.DrawString(ArialFont, coutOr, new Vector2(Position.X, Position.Y - 32), Color.Yellow);
        }

        private void GérerInput()
        {
            if (GestionInput.EstNouveauClicGauche() && TrouverSiIntersection())
            {
                FaireUpgrade();
            }
        }

        private void FaireUpgrade()
        {
            bool estUpgrader = false;

            foreach (Joueur j in Game.Components.OfType<Joueur>())
            {
                if (j.NombreDOR >= 10)
                {
                    for (int i = 0; i < tableauValeurNiveau.GetLength(0); i++)
                    {
                        if (tableauValeurNiveau[i, 0] == j.NiveauTempsRécolte && j.NiveauTempsRécolte != 5 && !estUpgrader)
                        {
                            SoundAchat.Play();
                            j.TempsCollectionRessource = tableauValeurNiveau[i + 1, 1];
                            j.NombreDOR -= (int)tableauValeurNiveau[i,2];
                            ++j.NiveauTempsRécolte;
                            Niveau = j.NiveauTempsRécolte;
                            estUpgrader = true;
                        }
                    }                  
                }
            }
        }
    }
}
