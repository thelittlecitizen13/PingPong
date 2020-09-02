using System;

namespace PingPong
{
    class Program
    {
        static void Main(string[] args)
        {

            PingPongServer server = new PingPongServer(8844);
            server.Run();
        }
    }
}
