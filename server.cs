using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace ChatServer
{
    public class Program
    {
        public static int SERVER_PORT = 12345;
        public static string SERVER_IP = "127.0.0.1";
        public static List<Client> allClients;

        public static int Main(string[] arg)
        {
            IPAddress localAddr = IPAddress.Parse(SERVER_IP);
            
            TcpListener server = new TcpListener(localAddr, SERVER_PORT);
            server.Start();
            allClients = new List<Client>(){};
            Console.WriteLine("Server up and running !");

            int counter = 0;

            //Thread pool
            while(true)
            {
                TcpClient IncomingClient = server.AcceptTcpClient();
                
                allClients.Add(new Client(counter, "Michel", IncomingClient));
                broadcastToAll(-1, "User#" + counter + " has connected !");
                counter++;
                //IncomingClient.Close();
            }
        }

        public static void broadcastToAll(int authorId, string message){
            Console.WriteLine(message);
            foreach (Client c in allClients)
            {
                if((c.id != authorId) && (c.client.Connected)){
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
                    NetworkStream ns = c.client.GetStream();
                    ns.Write(msg, 0, msg.Length);
                }
            }
        }
    }
}