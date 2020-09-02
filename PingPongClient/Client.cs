using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PingPongClient
{
    public class Client
    {
        private IPEndPoint serverEndPoint;
        private IPAddress serverIp;
        public Client(string ip, int port)
        {
            serverIp = IPAddress.Parse(ip);
            serverEndPoint = new IPEndPoint(serverIp, port);
        }
        public void Run()
        {
            try
            {
                // Create the socket to connect the remote server
                TcpClient sender = new TcpClient();
                sender.Connect(serverEndPoint);
                NetworkStream networkStream = sender.GetStream();
                Console.WriteLine("Connected to: {0}:{1} ",
                          ((IPEndPoint)sender.Client.RemoteEndPoint).Address,
                          ((IPEndPoint)sender.Client.RemoteEndPoint).Port);

                // Create the message to be sent
                Console.WriteLine("Please enter your message:");
                string msg = Console.ReadLine();
                byte[] messageSent = Encoding.ASCII.GetBytes(msg + " <EOF>");
                networkStream.Write(messageSent, 0, messageSent.Length);

                // Data buffer 
                byte[] messageReceived = new byte[1024];

                // Recieve a message from the server and print to console 
                int byteRecv = networkStream.Read(messageReceived);
                Console.WriteLine("Message from Server: {0}",
                      Encoding.ASCII.GetString(messageReceived,
                                                 0, byteRecv));

                // Close Socket  
                sender.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
        }
    }
}
