using Models;
using MQTestProject.Models;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMQTest
{
    /// <summary>
    /// Класс недописан. Начата реализация отправки объекта, но прекращено из-за отказа от ZeroMQ и NetMQ
    /// </summary>
    public class FrameClient
    {
        private readonly string _host;
        private readonly int _port;
        
        public FrameClient(string host, int port)
        {
            this._host = host;
            this._port = port;
        }

        public void StartSending()
        {
            Console.WriteLine("Enter city name\r\n");
            var city = Console.ReadLine() ?? "";
            using (var client = new RequestSocket())
            {
                client.Connect($"tcp://{_host}:{_port}");
                try
                {
                    while (true)
                    {
                        var weather = WeatherForecast.Get(city);
                        Send(client, weather);
                    }
                }
                catch (Exception ex)
                { 
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void Send(RequestSocket client, object message)
        {
            Console.WriteLine(" Sending message...");
            var netMqMessage = new NetMQMessage();
            netMqMessage.Append(new NetMQFrame(ObjectHelper.ObjectToByteArray(message)));
            client.SendMultipartMessage(netMqMessage);
                
            //client.SendFrame(ObjectHelper.ObjectToByteArray(message));
            var response = client.ReceiveFrameString();
            Console.WriteLine($" Response > {response}");
        }
    }
}
