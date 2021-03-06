namespace ActiveMQTest
{
    public class ActiveMQTestProgram
    {
        static readonly int _port = 61616;
        static readonly string _host = "localhost";
        public async Task Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Who we are? 1 - Simple Consumer, 2 - Simple Publisher, 3 - MassTransit Consumer, 4 - MassTransit Publiser");
            var key = Console.ReadKey().Key;
            Console.Write("\r\n");
            switch (key)
            {
                case ConsoleKey.D1: await StartSimpleConsumer(); break;
                case ConsoleKey.D2: await StartSimplePublisher(); break;
                case ConsoleKey.D3: await StartMassTransitConsumer(); break;
                case ConsoleKey.D4: await StartMassTransiPublisher(); break;
                default: Console.WriteLine("Unkown command"); break;
            }
        }

        private async Task StartMassTransiPublisher()
        {
            await MassTransitPublisher.Main(_host, _port);
        }

        private async Task StartMassTransitConsumer()
        {
            throw new NotImplementedException();
        }

        private async Task StartSimplePublisher()
        {
            using (var publisher = new ActiveMQTest.Standart.Publisher())
                await publisher.Main(_host, _port);
        }

        private async Task StartSimpleConsumer()
        {
            await new ActiveMQTest.Standart.Consumer().Main(_host, _port);
        }
    }
}