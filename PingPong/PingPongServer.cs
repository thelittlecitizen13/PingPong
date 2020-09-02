using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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
        public void RunWithTCPListener()
        {
            TcpListener listener = new TcpListener(localEndPoint.Address, localEndPoint.Port);
            Console.WriteLine($"Listening on {localEndPoint.Address}:{localEndPoint.Port}");
            Console.WriteLine("Waiting for connections");
            listener.Start();
            while (true)
            {
                //---incoming client connected---
                TcpClient client = listener.AcceptTcpClient();
                try
                {
                    Console.WriteLine("Connected to: {0}:{1} ",
                        ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(),
                        ((IPEndPoint)client.Client.RemoteEndPoint).Port.ToString());
                }
                catch
                {

                }
                object obj = new object();
                ThreadPool.QueueUserWorkItem(obj =>
                {
                    //---get the incoming data through a network stream---
                    NetworkStream nwStream = client.GetStream();
                    byte[] buffer = new byte[client.ReceiveBufferSize];

                    //---read incoming stream---
                    int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                    //---convert the data received into a string---
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received : " + dataReceived);

                    //---write back the text to the client---
                    Console.WriteLine("Sending back : " + dataReceived);
                    nwStream.Write(buffer, 0, bytesRead);
                    client.Close();
                },null);
            }
        server.Stop();
        }
        public void RunWithSockets()
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
                    try
                    {
                        Console.WriteLine("Connected to: {0} ",
                          clientSocket.RemoteEndPoint.ToString());
                    }
                    catch
                    {

                    }
                    object obj = new object();
                    ThreadPool.QueueUserWorkItem(obj =>
                    {
                        byte[] bytes = new Byte[1024];
                        string data = null;
                        while (true)
                        {
                            int numByte = clientSocket.Receive(bytes);

                            data += Encoding.ASCII.GetString(bytes,
                                                       0, numByte);
                            // PAY ATTENTION! the message from the client must have the content of "indexOf(here)" below to work!.
                            if (data.IndexOf("<EOF>") > -1)
                                break;
                        }
                        Console.WriteLine("Message received: {0} ", data);


                        // Send a message to Client 
                        byte[] message = Encoding.ASCII.GetBytes(data);
                        clientSocket.Send(message);

                        // Close client Socket so
                        // we can use the closed Socket  
                        // for a new Client Connection 
                        clientSocket.Shutdown(SocketShutdown.Both);
                        try
                        {
                            Console.WriteLine("Connection ended with {0} ",
                              clientSocket.RemoteEndPoint.ToString());
                        }
                        catch
                        {

                        }
                        clientSocket.Close();
                    }, null);
                    // Data Buffer
                    

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
