using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PingPong
{
    public class PingPongServer
    {
        private TcpListener server;
        private IPEndPoint localEndPoint;
        Socket listener;
        public PingPongServer(int port)
        {
            listener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1"); //Dns.GetHostEntry("localhost").AddressList[0];
            localEndPoint = new IPEndPoint(ipAddress, port);
            
        }
        public void Run()
        {
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                // Start listening for connections.
                while (true)
                {
                    Console.WriteLine("Waiting for connections");
                    Socket clientSocket = listener.Accept();

                    // Data Buffer
                    byte[] bytes = new Byte[1024];
                    string data = null;
                    while (true)
                    {
                        int numByte = clientSocket.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes,
                                                   0, numByte);

                        if (data.IndexOf("<EOF>") > -1)
                            break;
                    }
                    if (data.ToLower() == "exit")
                        break;
                    Console.WriteLine("Message received: {0} ", data);


                    // Send a message to Client 
                    byte[] message = Encoding.ASCII.GetBytes(data);
                    clientSocket.Send(message);

                    // Close client Socket so
                    // we can use the closed Socket  
                    // for a new Client Connection 
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();

                    ////---get the incoming data through a network stream---
                    //NetworkStream nwStream = client.GetStream();
                    //byte[] buffer = new byte[client.ReceiveBufferSize];

                    ////---read incoming stream---
                    //int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                    ////---convert the data received into a string---
                    //string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    //if (dataReceived.ToLower() == "exit")
                    //{
                    //    break;
                    //}
                    //Console.WriteLine("Received : " + dataReceived);

                    ////---write back the text to the client---
                    //Console.WriteLine("Sending back : " + dataReceived);
                    //nwStream.Write(buffer, 0, bytesRead);
                    //client.Close();
                }
                server.Stop();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
