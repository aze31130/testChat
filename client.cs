using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;

namespace ChatServer
{
    public class Client
    {
        public int id {get; set;}
        public string name {get; set;}
        public TcpClient client {get;set;}

        public Client(int id, string name, TcpClient client){
            this.id = id;
            this.name = name;
            this.client = client;

            Thread clientThread = new Thread(receiveMessage);
            clientThread.Start();
        }

        private void receiveMessage() {
            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String message = null;
            try {
                while(true){
                    NetworkStream clientConnection = this.client.GetStream();
                    int i;

                    while((i = clientConnection.Read(bytes, 0, bytes.Length))!=0)
                    {
                        // Translate data bytes to a ASCII string.
                        message = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Program.broadcastToAll(this.id, "<User#" + this.id + "> " + message);
                    }
                }
            } catch(Exception e){
                Program.broadcastToAll(-1, "User#" + this.id + " has disconnected !");
            }
        }
    }
}