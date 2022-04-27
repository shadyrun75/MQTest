namespace RabbitMQTest
{
    public class RabbitMQTestProgram
    { 
        readonly string _host = "localhost";
        readonly ushort _port = 5672;
        public async Task Main()
        {
            Console.Clear();
            Console.WriteLine("Who we are? 1 - Standart Consumer; 2 - Standart Publisher, 3 - MassTransit Consumer, 4 - MassTransit Publisher");
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
            await new RabbitMQTest.MassTransit.Publisher().Main(_host, _port);   
        }

        private async Task StartMassTransitConsumer()
        {
            await new RabbitMQTest.MassTransit.Consumer().Main(_host, _port);
        }

        private async Task StartSimplePublisher()
        {
            using (var publisher = new RabbitMQTest.Standart.Publisher())
                await publisher.Main(_host, _port);
        }

        private async Task StartSimpleConsumer()
        {
            await new RabbitMQTest.Standart.Consumer().Main(_host, _port);
        }
    }
}