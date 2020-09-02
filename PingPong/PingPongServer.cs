using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PingPong
{
    public class PingPongServer
    {
        private TcpListener server;
        private IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[0];
        TcpClient client;
        public PingPongServer(int port)
        {
            server = new TcpListener(ipAddress, port);
        }
        public void Run()
        {
            while(true)
            {
                //---incoming client connected---
                client = server.AcceptTcpClient();

                //---get the incoming data through a network stream---
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];

                //---read incoming stream---
                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                //---convert the data received into a string---
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                if (dataReceived.ToLower() == "exit")
                {
                    break;
                }
                Console.WriteLine("Received : " + dataReceived);

                //---write back the text to the client---
                Console.WriteLine("Sending back : " + dataReceived);
                nwStream.Write(buffer, 0, bytesRead);
                client.Close();
            }
            server.Stop();
        }
    }
}
