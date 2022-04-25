namespace ActiveMQTest
{
    public class ActiveMQTestProgram
    {
        static readonly int _port = 61616;
        static readonly string _host = "localhost";
        public static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Who we are? 1 - Simple Consumer, 2 - Simple Publisher, 3 - Object Consumer, 4 - Object Publiser");
            var key = Console.ReadKey().Key;
            Console.Write("\r\n");
            switch (key)
            {
                case ConsoleKey.D1: StartSimpleConsumer(); break;
                case ConsoleKey.D2: StartSimplePublisher(); break;
                case ConsoleKey.D3: StartObjectConsumer(); break;
                case ConsoleKey.D4: StartObjectPublisher(); break;
                default: Console.WriteLine("Unkown command"); break;
            }
        }

        private static void StartObjectPublisher()
        {
            throw new NotImplementedException();
        }

        private static void StartObjectConsumer()
        {
            throw new NotImplementedException();
        }

        private static void StartSimplePublisher()
        {
            SimplePublisher.Main(_host, _port);
        }

        private static void StartSimpleConsumer()
        {
            SimpleConsumer.Main(_host, _port);
        }
    }
}