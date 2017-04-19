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
using System.IO;
using Lidgren.Network;

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
        bool PartieCommencée { get; set; }
        bool MondeGénéré { get; set; }
        GridDeJeu GridDeJeu { get; set; }
        GenerateurProcedural GenProc { get; set; }
        ControlePhaseDeJeu ControlePhase { get; set; }
        public static EnemySpawner Spawner { get; set; }
        StreamWriter sWriter { get; set; }


        //Network
        string locationFichierIPNom = "..\\..\\..\\..\\Projet-Int-gration\\Atelier 15\\Atelier 15\\bin\\x86\\Debug\\FichierInfoLogin.txt";
        static NetClient Client2 { get; set; }
        string HostIP { get; set; }
        string ClientName = "player ";
        static List<JoueurConnection> ListJoueur;
        static System.Timers.Timer update;
        int NombreJoueurConnecté { get; set; }

        public Atelier()
        {
            Content.RootDirectory = "Content";
            PériphériqueGraphique = new GraphicsDeviceManager(this);
            PériphériqueGraphique.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            IsMouseVisible = true;
            PériphériqueGraphique.PreferredBackBufferWidth = 1280;
            PériphériqueGraphique.PreferredBackBufferHeight = 720;
            PériphériqueGraphique.IsFullScreen = false;

        }

        protected override void Initialize()
        {
            try
            {
                RechercheIPEtNom();
                RejoindreUneConnection(ClientName);
            }
            catch(Exception) { }
            InstantiationDesServices();
            CréationDuJeu();
            base.Initialize();
        }


        private void InstantiationDesServices()
        {
            Vector3 positionCaméra = new Vector3(0, 100, 250);
            Vector3 cibleCaméra = new Vector3(0, 0, -10);
            EffetLumiere = new BasicEffect(GraphicsDevice);
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
            try
            {
                Services.AddService(typeof(NetClient), Client2);
            }
            catch(Exception) { }
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
            Spawner = new EnemySpawner(this);
            Components.Add(Spawner);
        }


        //Network

        private void RechercheIPEtNom()
        {
            StreamReader sReader = new StreamReader(locationFichierIPNom);
            HostIP = sReader.ReadLine();
            sReader.Close();
        }

        enum PacketTypes
        {
            LOGIN,
            WORLDSTATE,
            STARTGAME,
            ENEMY
        }

        private void RejoindreUneConnection(string name)
        {
            //network connection
            NetPeerConfiguration config = new NetPeerConfiguration("game");
            Client2 = new NetClient(config);
            NetOutgoingMessage outmsg = Client2.CreateMessage();
            Client2.Start();
            outmsg.Write((byte)PacketTypes.LOGIN);
            outmsg.Write(name);
            Client2.Connect(HostIP, 5009, outmsg);
            ListJoueur = new List<JoueurConnection>();
            update = new System.Timers.Timer(50);
            update.Elapsed += new System.Timers.ElapsedEventHandler(update_Elapsed);
            WaitForStartingInfo();
            update.Start();
        }
        private static void WaitForStartingInfo()
        {
            // When this is set to true, we are approved and ready to go
            bool CanStart = false;

            // New incomgin message
            NetIncomingMessage inc;

            // Loop untill we are approved
            while (!CanStart)
            {

                // If new messages arrived
                if ((inc = Client2.ReadMessage()) != null)
                {
                    // Switch based on the message types
                    switch (inc.MessageType)
                    {

                        // All manually sent messages are type of "Data"
                        case NetIncomingMessageType.Data:
                            // Read the first byte
                            // This way we can separate packets from each others
                            if (inc.ReadByte() == (byte)PacketTypes.WORLDSTATE)
                            {
                                // Worldstate packet structure
                                //
                                // int = count of players
                                // character obj * count



                                //Console.WriteLine("WorldState Update");

                                // Empty the gamestatelist
                                // new data is coming, so everything we knew on last frame, does not count here
                                // Even if client would manipulate this list ( hack ), it wont matter, becouse server handles the real list
                                ListJoueur.Clear();

                                // Declare count
                                int count = 0;

                                // Read int
                                //comment de plus
                                count = inc.ReadInt32();
                                // Iterate all players
                                for (int i = 0; i < count; i++)
                                {

                                    // Create new character to hold the data
                                    JoueurConnection ch = new JoueurConnection();

                                    // Read all properties ( Server writes characters all props, so now we can read em here. Easy )
                                    inc.ReadAllProperties(ch);

                                    // Add it to list
                                    ListJoueur.Add(ch);
                                }

                                // When all players are added to list, start the game
                                CanStart = true;
                            }
                            break;

                        default:
                            // Should not happen and if happens, don't care
                            break;
                    }
                }
            }
        }

        private void CheckServerMessages()
        {
            // Create new incoming message holder
            NetIncomingMessage inc;

            // While theres new messages
            //
            // THIS is exactly the same as in WaitForStartingInfo() function
            // Check if its Data message
            // If its WorldState, read all the characters to list
            while ((inc = Client2.ReadMessage()) != null)
            {
                if (inc.MessageType == NetIncomingMessageType.Data)
                {
                    byte type = inc.ReadByte();

                    if (type == (byte)PacketTypes.WORLDSTATE)
                    {
                        Console.WriteLine("World State uppaus");
                        ListJoueur.Clear();
                        int jii = 0;
                        jii = inc.ReadInt32();
                        for (int i = 0; i < jii; i++)
                        {
                            JoueurConnection ch = new JoueurConnection();
                            inc.ReadAllProperties(ch);
                            ListJoueur.Add(ch);
                        }
                    }
                    else
                    {
                        if (type == (byte)PacketTypes.ENEMY)
                        {
                            Spawner.CréerEnemy(inc.ReadInt32());
                        }
                    }
                }
            }
        }

        void update_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Check if server sent new messages
            CheckServerMessages();
        }
    }
}

