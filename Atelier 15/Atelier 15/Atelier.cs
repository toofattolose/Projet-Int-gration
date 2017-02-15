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
    public class Atelier : Microsoft.Xna.Framework.Game
    {
        const float INTERVALLE_CALCUL_FPS = 1f;
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        GraphicsDeviceManager PériphériqueGraphique { get; set; }
        GraphicsDevice Peripherique { get; set; }
        SpriteBatch GestionSprites { get; set; }
        BasicEffect EffetLumiere { get; set; }
        Caméra CaméraJeu { get; set; }
        InputManager GestionInput { get; set; }
        Ennemis Ennemi { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        int i { get; set; }

        public Atelier()
        {
            Content.RootDirectory = "Content";
            PériphériqueGraphique = new GraphicsDeviceManager(this);
            PériphériqueGraphique.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Vector3 positionCaméra = new Vector3(0, 100, 250);
            Vector3 cibleCaméra = new Vector3(0, 0, -10);
            EffetLumiere = new BasicEffect(GraphicsDevice);
            i = 0;



            GestionInput = new InputManager(this);
            Components.Add(GestionInput);

            //Grid de jeu
            GridDeJeu gridDeJeu = new GridDeJeu(this, new Vector3(256, 25, 256), new Vector2(64, 64));
            Components.Add(gridDeJeu);
            Services.AddService(typeof(GridDeJeu), gridDeJeu);
            

            Components.Add(new AfficheurFPS(this, "Arial20", Color.Red, INTERVALLE_CALCUL_FPS));
            Services.AddService(typeof(Random), new Random());
            Services.AddService(typeof(RessourcesManager<SpriteFont>), new RessourcesManager<SpriteFont>(this, "Fonts"));
            Services.AddService(typeof(RessourcesManager<Texture2D>), new RessourcesManager<Texture2D>(this, "Textures"));
            Services.AddService(typeof(RessourcesManager<Model>), new RessourcesManager<Model>(this, "Models"));
            Services.AddService(typeof(InputManager), GestionInput);
            GestionSprites = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), GestionSprites);

            //Création des composants de base
            Components.Add(new Afficheur3D(this));
            Components.Add(new Terrain(this, 1f, Vector3.Zero, Vector3.Zero, new Vector3(256, 25, 256), new Vector2(64, 64), INTERVALLE_MAJ_STANDARD));
            GenerateurProcedural generateurProc = new GenerateurProcedural(this, Vector3.Zero, new Vector3(256, 25, 256), new Vector2(64, 64));
            ControlePhaseDeJeu controlePhase = new ControlePhaseDeJeu(this, 180f, 120f);
            Components.Add(controlePhase);
            Services.AddService(typeof(ControlePhaseDeJeu), controlePhase);
            CaméraJeu = new Caméra3rdPerson(this, positionCaméra, cibleCaméra, Vector3.Up, INTERVALLE_MAJ_STANDARD);
            Ennemi = new Ennemis(this, "player", 0.01f, new Vector3(256 / 2f, 0, 256 / 2f), Vector3.Zero);
            Services.AddService(typeof(Caméra), CaméraJeu);
            Components.Add(generateurProc);
            Components.Add(CaméraJeu);
            
            PathFinding pathFinding = new PathFinding(this);
            Components.Add(pathFinding);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            GérerClavier();
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= 5)
            {
                Components.Add(new Ennemis(this, "player", 0.01f, new Vector3(256 / 2f + i, 0, 256 / 2f), Vector3.Zero));
                ++i;
                TempsÉcouléDepuisMAJ = 0;
               
            }
            base.Update(gameTime);
        }

        private void GérerClavier()
        {
            if (GestionInput.EstEnfoncée(Keys.Escape))
            {
                Exit();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}

