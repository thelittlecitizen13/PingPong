using System.Threading;

namespace PingPongClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(1000);
            Client client = new Client(args[0], int.Parse(args[1]));
            client.Run();
        }
    }

}
