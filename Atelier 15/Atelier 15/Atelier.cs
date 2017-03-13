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
using Lidgren.Network;

namespace AtelierXNA
{
    public class Atelier : Microsoft.Xna.Framework.Game
    {
        //Network
        static NetClient Client;
        static List<JoueurConnection> ListJoueur;
        static System.Timers.Timer update;


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
        string HostIp { get; set; }
        bool PartieCommencée { get; set; }
        bool MondeGénéré { get; set; }
        GridDeJeu GridDeJeu { get; set; }
        GenerateurProcedural GenProc { get; set; }
        ControlePhaseDeJeu ControlePhase { get; set; }

        public Atelier()
        {
            Content.RootDirectory = "Content";
            PériphériqueGraphique = new GraphicsDeviceManager(this);
            PériphériqueGraphique.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            IsMouseVisible = true;

            //network connection
            //NetPeerConfiguration config = new NetPeerConfiguration("game");
            //Client = new NetClient(config);
            //NetOutgoingMessage outmsg = Client.CreateMessage();
            //Client.Start();
            //outmsg.Write((byte)PacketTypes.LOGIN);
            //outmsg.Write("myName");
            //Client.Connect(HostIp, 5009, outmsg);


        }

        protected override void Initialize()
        {
            InstantiationDesServices();
            CréationDuJeu();
            base.Initialize();
        }

        private void InstantiationDesServices()
        {
            Vector3 positionCaméra = new Vector3(0, 100, 250);
            Vector3 cibleCaméra = new Vector3(0, 0, -10);
            EffetLumiere = new BasicEffect(GraphicsDevice);
            i = 0;
            EffetLumiere = new BasicEffect(GraphicsDevice);
            GridDeJeu = new GridDeJeu(this, new Vector3(256, 25, 256), new Vector2(64, 64));
            Services.AddService(typeof(GridDeJeu), GridDeJeu);
            GestionInput = new InputManager(this);
            Services.AddService(typeof(InputManager), GestionInput);
            Components.Add(new AfficheurFPS(this, "Arial20", Color.Red, INTERVALLE_CALCUL_FPS));
            Services.AddService(typeof(Random), new Random());
            Services.AddService(typeof(RessourcesManager<SpriteFont>), new RessourcesManager<SpriteFont>(this, "Fonts"));
            Services.AddService(typeof(RessourcesManager<Texture2D>), new RessourcesManager<Texture2D>(this, "Textures"));
            Services.AddService(typeof(RessourcesManager<Model>), new RessourcesManager<Model>(this, "Models"));
            GestionSprites = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), GestionSprites);

            //Création des composants de base
            //Components.Add(new Afficheur3D(this));
            //Components.Add(new Terrain(this, 1f, Vector3.Zero, Vector3.Zero, new Vector3(256, 25, 256), new Vector2(64, 64), INTERVALLE_MAJ_STANDARD));
            //GenerateurProcedural generateurProc = new GenerateurProcedural(this, Vector3.Zero, new Vector3(256, 25, 256), new Vector2(64, 64));
            //ControlePhaseDeJeu controlePhase = new ControlePhaseDeJeu(this, 120f, 120f);
            //Components.Add(controlePhase);
            //Services.AddService(typeof(ControlePhaseDeJeu), controlePhase);
            //CaméraJeu = new Caméra3rdPerson(this, positionCaméra, cibleCaméra, Vector3.Up, INTERVALLE_MAJ_STANDARD);
            //Services.AddService(typeof(Caméra), CaméraJeu);
            //Components.Add(generateurProc);
            //Components.Add(CaméraJeu);      
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            GérerClavier();
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

        private void CréationDuJeu()
        {
            Vector3 positionCaméra = new Vector3(0, 100, 250);
            Vector3 cibleCaméra = new Vector3(0, 0, -10);
            //Grid de jeu
            Components.Add(GridDeJeu);            
            Components.Add(GestionInput);
            //Création des composants de base
            Components.Add(new Afficheur3D(this));
            CaméraJeu = new Caméra3rdPerson(this, positionCaméra, cibleCaméra, Vector3.Up, INTERVALLE_MAJ_STANDARD);
            Services.AddService(typeof(Caméra), CaméraJeu);
            Components.Add(CaméraJeu);
            Components.Add(new Terrain(this, 1f, Vector3.Zero, Vector3.Zero, new Vector3(256, 25, 256), new Vector2(64, 64), INTERVALLE_MAJ_STANDARD));   
            GenProc = new GenerateurProcedural(this, Vector3.Zero, new Vector3(256, 25, 256), new Vector2(64, 64));
            Components.Add(GenProc);
        }
        
        enum PacketTypes
        {
            LOGIN,
            MOVE,
            WORLDSTATE
        }



        private void RejoindreUneConnection()
        {
            //network connection
            NetPeerConfiguration config = new NetPeerConfiguration("game");
            Client = new NetClient(config);
            NetOutgoingMessage outmsg = Client.CreateMessage();
            Client.Start();
            outmsg.Write((byte)PacketTypes.LOGIN);
            outmsg.Write("myName");
            Client.Connect(HostIp, 5009, outmsg);
        }

    }
}

