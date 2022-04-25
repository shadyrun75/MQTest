using NetMQ;

namespace NetMQTest
{
    /// <summary>
    /// Программа запуска тестов брокера сообщений ZeroMQ и NetMQ.
    /// По итогу не пригодно для целей компании из-за большого объема доработки и большого количества "заморочек" при внедрении, таких как
    /// использование в своих потоках требует запускать процедуры отправки/получения из главного потока; написание по сути своего брокера,
    /// т.к. фильтрация по consumer будет лишь вручную в коде; всегда надо знать адреса серверов; надо очищать память, иначе будут критические ошибки.
    /// </summary>
    public class NetMQTestProgram
    {
        static readonly int _port = 5556;
        static readonly string _host = "localhost";
        
        public static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Who we are? 1 - Simple Server, 2 - Simple Client, 3 - Frame Server, 4 - Frame Client");
            var key = Console.ReadKey().Key;
            Console.Write("\r\n");
            switch (key)
            {
                case ConsoleKey.D1: StartSimpleServer(); break;
                case ConsoleKey.D2: StartSimpleClient(); break;
                case ConsoleKey.D3: StartFrameServer(); break;
                case ConsoleKey.D4: StartFrameClient(); break;
                default: Console.WriteLine("Unkown command"); break;
            }
        }

        private static void StartSimpleServer()
        {
            SimpleServer server = new SimpleServer(_port);
            server.Start();
        }
        private static void StartSimpleClient()
        {
            SimpleClient client = new(_host, _port);
            client.Start();
        }

        private static void StartFrameServer()
        {
            FrameServer server = new FrameServer(_port);
            server.Start();
        }

        private static void StartFrameClient()
        {
            FrameClient client = new FrameClient(_host, _port);
            client.StartSending();
        }
    }
}