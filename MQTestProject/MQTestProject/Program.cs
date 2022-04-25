using NetMQTest;
using ActiveMQTest;

Console.WriteLine("What we gonna test? 1 - NetMQ, 2 - ActiveMQ ");
try
{
    switch (Console.ReadKey().Key)
    {
        case ConsoleKey.D1: NetMQTestProgram.Main(null); break;
        case ConsoleKey.D2: ActiveMQTestProgram.Main(null); break;
        default: Console.WriteLine("Unkown command"); break;
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
Console.WriteLine("Press enter to exit...");
Console.ReadLine();