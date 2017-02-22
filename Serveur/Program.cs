﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace AtelierXNA
{
    class Program
    {
        // Server object
        static NetServer Server;
        // Configuration object
        static NetPeerConfiguration Config;

        static void Main(string[] args)
        {
            int valeurNom = 0;
            // Create new instance of configs. Parameter is "application Id". It has to be same on client and server.
            Config = new NetPeerConfiguration("game");

            // Set server port
            Config.Port = 5009;

            // Max client amount
            Config.MaximumConnections = 4;

            // Enable New messagetype. Explained later
            Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            // Create new server based on the configs just defined
            Server = new NetServer(Config);

            // Start it
            Server.Start();

            // Eh..
            Console.WriteLine("Server Started");

            // Create list of "Characters" ( defined later in code ). This list holds the world state. Character positions
            List<JoueurConnection> GameWorldState = new List<JoueurConnection>();

            // Object that can be used to store and read messages
            NetIncomingMessage inc;

            // Check time
            DateTime time = DateTime.Now;

            // Create timespan of 30ms
            TimeSpan timetopass = new TimeSpan(0, 0, 0, 0, 30);

            // Write to con..
            Console.WriteLine("Waiting for new connections and updateing world state to current ones");

            // Main loop
            // This kind of loop can't be made in XNA. In there, its basically same, but without while
            // Or maybe it could be while(new messages)
            while (true)
            {
                // Server.ReadMessage() Returns new messages, that have not yet been read.
                // If "inc" is null -> ReadMessage returned null -> Its null, so dont do this :)
                if ((inc = Server.ReadMessage()) != null)
                {
                    // Theres few different types of messages. To simplify this process, i left only 2 of em here
                    switch (inc.MessageType)
                    {
                        // If incoming message is Request for connection approval
                        // This is the very first packet/message that is sent from client
                        // Here you can do new player initialisation stuff
                        case NetIncomingMessageType.ConnectionApproval:

                            // Read the first byte of the packet
                            // ( Enums can be casted to bytes, so it be used to make bytes human readable )
                            if (inc.ReadByte() == (byte)PacketTypes.LOGIN)
                            {
                                Console.WriteLine("Incoming LOGIN");

                                // Approve clients connection ( Its sort of agreenment. "You can be my client and i will host you" )
                                inc.SenderConnection.Approve();

                                // Init random
                                ++valeurNom;

                                // Add new character to the game.
                                // It adds new player to the list and stores name, ( that was sent from the client )
                                // Random x, y and stores client IP+Port
                                GameWorldState.Add(new JoueurConnection("Player"+valeurNom.ToString(), inc.SenderConnection));

                                // Create message, that can be written and sent
                                NetOutgoingMessage outmsg = Server.CreateMessage();

                                // first we write byte
                                outmsg.Write((byte)PacketTypes.WORLDSTATE);

                                // then int
                                outmsg.Write(GameWorldState.Count);

                                // iterate trought every character ingame
                                foreach (JoueurConnection joueur in GameWorldState)
                                {
                                    // This is handy method
                                    // It writes all the properties of object to the packet
                                    outmsg.WriteAllProperties(joueur);
                                }

                                // Now, packet contains:
                                // Byte = packet type
                                // Int = how many players there is in game
                                // character object * how many players is in game

                                // Send message/packet to all connections, in reliably order, channel 0
                                // Reliably means, that each packet arrives in same order they were sent. Its slower than unreliable, but easyest to understand
                                Server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);

                                // Debug
                                Console.WriteLine("Approved new connection and updated the world status");
                            }

                            break;
                        // Data type is all messages manually sent from client
                        // ( Approval is automated process )
                        case NetIncomingMessageType.Data:

                            // Read first byte
                            if (inc.ReadByte() == (byte)PacketTypes.SCORE)
                            {
                                // Check who sent the message
                                // This way we know, what character belongs to message sender
                                foreach (JoueurConnection ch in GameWorldState)
                                {
                                    // If stored connection ( check approved message. We stored ip+port there, to character obj )
                                    // Find the correct character
                                    if (ch.Connection != inc.SenderConnection)
                                        continue;

                                    // Read next byte
                                    int score = inc.ReadInt32();

                                    // Create new message
                                    NetOutgoingMessage outmsg = Server.CreateMessage();

                                    // Write byte, that is type of world state
                                    //outmsg.Write((byte)PacketTypes.WORLDSTATE);

                                    // Write int, "how many players in game?"
                                    //outmsg.Write(GameWorldState.Count);

                                    // Iterate throught all the players in game
                                    foreach (JoueurConnection ch2 in GameWorldState)
                                    {
                                        // Write all the properties of object to message
                                        //outmsg.WriteAllProperties(ch2);
                                        outmsg.Write(ch2.Score);
                                    }

                                    // Message contains
                                    // Byte = PacketType
                                    // Int = Player count
                                    // Character obj * Player count

                                    // Send messsage to clients ( All connections, in reliable order, channel 0)
                                    Server.SendMessage(outmsg, Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                                    break;
                                }

                            }
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            // In case status changed
                            // It can be one of these
                            // NetConnectionStatus.Connected;
                            // NetConnectionStatus.Connecting;
                            // NetConnectionStatus.Disconnected;
                            // NetConnectionStatus.Disconnecting;
                            // NetConnectionStatus.None;

                            // NOTE: Disconnecting and Disconnected are not instant unless client is shutdown with disconnect()
                            Console.WriteLine(inc.SenderConnection.ToString() + " status changed. " + (NetConnectionStatus)inc.SenderConnection.Status);
                            if (inc.SenderConnection.Status == NetConnectionStatus.Disconnected || inc.SenderConnection.Status == NetConnectionStatus.Disconnecting)
                            {
                                // Find disconnected character and remove it
                                foreach (JoueurConnection cha in GameWorldState)
                                {
                                    if (cha.Connection == inc.SenderConnection)
                                    {
                                        GameWorldState.Remove(cha);
                                        break;
                                    }
                                }
                            }
                            break;
                        default:
                            // As i statet previously, theres few other kind of messages also, but i dont cover those in this example
                            // Uncommenting next line, informs you, when ever some other kind of message is received
                            //Console.WriteLine("Not Important Message");
                            break;
                    }
                } // If New messages

                // if 30ms has passed
                if ((time + timetopass) < DateTime.Now)
                {
                    // If there is even 1 client
                    if (Server.ConnectionsCount != 0)
                    {
                        // Create new message
                        NetOutgoingMessage outmsg = Server.CreateMessage();

                        // Write byte
                        outmsg.Write((byte)PacketTypes.WORLDSTATE);

                        // Write Int
                        outmsg.Write(GameWorldState.Count);

                        // Iterate throught all the players in game
                        foreach (JoueurConnection ch2 in GameWorldState)
                        {

                            // Write all properties of character, to the message
                            outmsg.WriteAllProperties(ch2);
                        }

                        // Message contains
                        // byte = Type
                        // Int = Player count
                        // Character obj * Player count

                        // Send messsage to clients ( All connections, in reliable order, channel 0)
                        Server.SendMessage(outmsg, Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                    }
                    // Update current time
                    time = DateTime.Now;
                }

                // While loops run as fast as your computer lets. While(true) can lock your computer up. Even 1ms sleep, lets other programs have piece of your CPU time
                //System.Threading.Thread.Sleep(1);
            }
        }
    }


    // This is good way to handle different kind of packets
    // there has to be some way, to detect, what kind of packet/message is incoming.
    // With this, you can read message in correct order ( ie. Can't read int, if its string etc )

    // Best thing about this method is, that even if you change the order of the entrys in enum, the system won't break up
    // Enum can be casted ( converted ) to byte
    enum PacketTypes
    {
        LOGIN,
        SCORE,
        WORLDSTATE
    }
    //class LoginPacket
    //{
    //    public string MyName { get; set; }
    //    public LoginPacket(string name)
    //    {
    //        MyName = name;
    //    }
    //}
}