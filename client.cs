using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace ChatServer
{
    public class Client
    {
        public int id {get; set;}
        public Socket client {get;set;}

        public Client(int id, Socket client){
            this.id = id;
            this.client = client;

            Thread clientThread = new Thread(receiveMessage);
            clientThread.Start();
        }

        private void receiveMessage() {
            // Buffer for reading data
            byte[] bytes = new Byte[256];
            string message = null;
            try {
                while(true){
                    while(true){                                                        //building the word from buffer
                        int messageLength = client.Receive(bytes);
                        message = Encoding.ASCII.GetString(bytes,0,messageLength);
                        if(message.IndexOf("<EOF>") > -1){
                            break;
                        }
                    }
                    message = message.Replace("<EOF>", "");                            //removing <EOF> sequence and printing the word
                    message = "<User#" + this.id + "> " + message;
                    Program.broadcastToAll(this.id, message);
                }
            } catch(Exception){
                Program.broadcastToAll(-1, "User#" + this.id + " has disconnected !");
            }
        }
    }
}