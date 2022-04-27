using NetMQTest;
using ActiveMQTest;

Console.WriteLine("What we gonna test? 1 - RabbitMQ, 2 - ActiveMQ, 3 - ZeroMQ ");
try
{
    switch (Console.ReadKey().Key)
    {
        case ConsoleKey.D1: await new RabbitMQTest.RabbitMQTestProgram().Main(); break;
        case ConsoleKey.D2: await new ActiveMQTestProgram().Main(null); break;
        case ConsoleKey.D3: NetMQTestProgram.Main(null); break;
        default: Console.WriteLine("Unkown command"); break;
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
Console.WriteLine("Press enter to exit...");
Console.ReadLine();