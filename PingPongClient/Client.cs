using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using PingPong.Data;

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
                
                Console.WriteLine("Connected to: {0}:{1} ",
                          ((IPEndPoint)sender.Client.RemoteEndPoint).Address,
                          ((IPEndPoint)sender.Client.RemoteEndPoint).Port);

                // Create the message to be sent
                Console.WriteLine("Please enter person`s name:");
                string name = Console.ReadLine();
                Console.WriteLine("Please enter person`s age:");
                int age = int.Parse(Console.ReadLine());
                Person sendPerson = new Person(name, age);

                NetworkStream networkStream = sender.GetStream();
                IFormatter formatter = new BinaryFormatter(); // the formatter that will serialize my object on my stream 
                //byte[] messageSent = Encoding.ASCII.GetBytes(msg + " <EOF>");
                //networkStream.Write(messageSent, 0, messageSent.Length);
                formatter.Serialize(networkStream, sendPerson);

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
