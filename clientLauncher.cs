using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;

namespace ChatClient
{
    public class Program
    {
        public static int SERVER_PORT = 12345;
        public static string SERVER_IP = "127.0.0.1";
        public static NetworkStream connection;

        public static int Main(string[] arg)
        {
            TcpClient client = new TcpClient(SERVER_IP, SERVER_PORT);
            connection = client.GetStream();

            Thread send = new Thread(SendMessage);
            Thread receive = new Thread(ReceiveMessage);

            send.Start();
            receive.Start();
            return 0;
        }

        private static void SendMessage(){
            while(true){
                string message = Console.ReadLine();

                Byte[] dataToSend = System.Text.Encoding.ASCII.GetBytes(message);
                connection.Write(dataToSend, 0, dataToSend.Length);
            }
        }

        private static void ReceiveMessage(){
            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String message = null;

            while(true){
                int i;
                while((i = connection.Read(bytes, 0, bytes.Length))!=0)
                {
                    //Translate data bytes to a ASCII string.
                    message = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine(message);
                }
            }
        }
    }
}