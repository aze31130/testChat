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
        public static List<Client> allClients;

        public static int Main(string[] arg)
        {
            //endpoint (ip + port)
            IPHostEntry ipHostInfo = Dns.GetHostEntry("127.0.0.1");  
            IPAddress ipAddress = ipHostInfo.AddressList[0];  
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, SERVER_PORT); 

            allClients = new List<Client>(){};
            Socket server = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); 
            server.Bind(localEndPoint);
            server.Listen(5);

            int counter = 0;
            Console.WriteLine("Server up and running !");
            while(true){
                //instruction blocante
                Socket client = server.Accept();

                allClients.Add(new Client(counter, client));
                broadcastToAll(-1, "User#" + counter + " has connected !");
                counter++;
                
            }
        }

        public static void broadcastToAll(int authorId, string message){
            Console.WriteLine(message);
            foreach (Client c in allClients)
            {
                if((c.id != authorId) && (c.client.Connected)){
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(message + "<EOF>");
                    c.client.Send(msg);
                }
            }
        }
    }
}