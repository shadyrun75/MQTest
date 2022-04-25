using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMQTest
{
    public class SimpleServer
    {
        private readonly int _port;
        private bool stop = true;
        CancellationTokenSource cancellationToken = new CancellationTokenSource();
        public SimpleServer(int port)
        {
            _port = port;
        }

        public void Start()
        {
            using (var runtime = new NetMQRuntime())
                runtime.Run(Run(), Exit());
        }

        private async Task Run()
        {
            using (var server = new ResponseSocket("inproc://async"))
            {
                server.Bind($"tcp://*:{_port}");
                stop = false;
                while (true)
                {
                    var (message, _) = await server.ReceiveFrameStringAsync(cancellationToken.Token);
                    Console.WriteLine($" New data > {message}");
                    Thread.Sleep(100);
                    server.SendFrame("Confirm");
                    if (stop)
                        break;
                }
            }
        }

        private async Task Exit()
        {
            try
            {
                Console.WriteLine("Press enter to stop server");

                await Task.Run(() => Console.ReadLine());
                cancellationToken.Cancel();
            }
            finally
            {
                stop = true;
            }
        }

    }
}
