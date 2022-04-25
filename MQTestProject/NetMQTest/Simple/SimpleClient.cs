using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMQTest
{
    public class SimpleClient
    {
        private readonly string _host;
        private readonly int _port;

        public SimpleClient(string host, int port)
        {
            this._host = host;
            this._port = port;
        }

        public void Start()
        {
            while (true)
            {
                Console.WriteLine("Enter message \r\n > ");
                var value = Console.ReadLine() ?? "quit";
                if (value == "quit")
                {
                    Console.WriteLine(" Stoping client");
                    break;
                }
                Send(value);
            }
        }

        public void Send(string message)
        {
            using (var client = new RequestSocket())
            {
                client.Connect($"tcp://{_host}:{_port}");
                Console.WriteLine(" Sending message...");
                client.SendFrame(message);
                var response = client.ReceiveFrameString();
                Console.WriteLine($" Response > {response}");
            }
        }
    }
}
