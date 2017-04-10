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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class EnemyIcon : UpgradeIcon
    {
        int Niveau { get; set; }
        int Prix { get; set; }
        SpriteFont Arial { get; set; }
        NetClient Client { get; set; }

        public EnemyIcon(Game game, Vector2 position, string location, int niveau, int prix)
            : base(game, position, location)
        {
            // TODO: Construct any child components here
            Niveau = niveau;
            Prix = prix;
        }

        public override void Initialize()
        {
            base.Initialize();
            Arial = Game.Content.Load<SpriteFont>("Fonts/Arial20");
            Client = Game.Services.GetService(typeof(NetClient)) as NetClient;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            EstCliqué();
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprite.Begin();
            AfficherNiveauEtPrix();
            GestionSprite.End();
            base.Draw(gameTime);
        }

        private void AfficherNiveauEtPrix()
        {
            string niveauEnemy = "Niveau " + Niveau.ToString();
            string prixEnemy = Prix.ToString() + " Or";
            GestionSprite.DrawString(Arial, niveauEnemy, Position + new Vector2(76, 0), Color.White);
            GestionSprite.DrawString(Arial, prixEnemy, Position + new Vector2(76, 32), Color.Yellow);
        }

        public void EstCliqué()
        {
            if (GestionInput.EstNouveauClicGauche() && TrouverSiIntersection())
            {
                foreach(Joueur j in Game.Components.OfType<Joueur>())
                {
                    if (j.NombreDOR >= Prix)
                    {
                        NetOutgoingMessage outmsg = Client.CreateMessage();
                        SoundAchat.Play();
                        outmsg.Write((byte)PacketTypes.ENEMY);
                        outmsg.Write(Niveau);
                        Client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
                        j.NombreDOR -= Prix;
                    }
                }
                
            }
        }

        enum PacketTypes
        {
            LOGIN,
            WORLDSTATE,
            STARTGAME,
            ENEMY
        }

    }
}
