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
                Socket sender = new Socket(serverIp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sender.Connect(serverEndPoint);
                Console.WriteLine("Connected to: {0} ",
                          sender.RemoteEndPoint.ToString());

                // Create the message to be sent
                Console.WriteLine("Please enter your message:");
                string msg = Console.ReadLine();
                byte[] messageSent = Encoding.ASCII.GetBytes(msg + " <EOF>");
                int byteSent = sender.Send(messageSent);

                // Data buffer 
                byte[] messageReceived = new byte[1024];

                // Recieve a message from the server and print to console 
                int byteRecv = sender.Receive(messageReceived);
                Console.WriteLine("Message from Server -> {0}",
                      Encoding.ASCII.GetString(messageReceived,
                                                 0, byteRecv));

                // Close Socket  
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
        }
    }
}
