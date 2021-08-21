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
        public static Socket socket;

        public static int Main(string[] arg)
        {
            try {
                socket = ConnectServer(SERVER_IP, SERVER_PORT);

                
                Thread send = new Thread(SendMessage);
                Thread receive = new Thread(ReceiveMessage);
                receive.Start();
                send.Start();
            } catch(Exception e){
                Console.Error.WriteLine(e.ToString());
            }
            


            /*
                client
                essayer connecter sur une ip + port [fonction ConnectSocket / ConnectServer]
                    ==> soit il y arrive OK
                    ==> soit impossible ERROR


                client arrive a se connecter
                    ==> "ecouter les entrée utilisateurs" Console.ReadLine puis les envoyer [Thread1]
                    ==> "ecouter le serveur" quand quelqu'un envoie un message, on veut le recevoir aussi [Thread2]


            */
            return 0;
        }

        public static Socket ConnectServer(string ip, int port){
            IPHostEntry host = Dns.GetHostEntry(ip);

            //On essaye de trouver un endpoint
            foreach(IPAddress ipa in host.AddressList){
                IPEndPoint ipe = new IPEndPoint(ipa, port);

                Socket potentialSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                potentialSocket.Connect(ipe);

                if(potentialSocket.Connected){
                    return potentialSocket;
                }
            }
            return null;
        }

        private static void SendMessage(){
            string message;
            while(true){
                message = Console.ReadLine();
                Byte[] dataToSend = System.Text.Encoding.ASCII.GetBytes(message + "<EOF>");
                socket.Send(dataToSend);
            }
        }
        
        private static void ReceiveMessage() {
            // Buffer for reading data
            byte[] bytes = new Byte[256];
            string message = null;
            try {
                while(true){
                    while(true){                                                      //building the word from buffer
                        int messageLength = socket.Receive(bytes);
                        message = Encoding.ASCII.GetString(bytes,0,messageLength);
                        
                        if(message.IndexOf("<EOF>") > -1){
                            break;
                        }
                    }
                    message = message.Replace("<EOF>", "");                            //removing <EOF> sequence and printing the word
                    Console.WriteLine(message);
                    message = "";
                }
            } catch(Exception e){
                Console.Error.WriteLine(e.ToString());
            }
        }
    }
}