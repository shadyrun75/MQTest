using Apache.NMS;
using Apache.NMS.Util;
using MQTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveMQTest.Standart
{
    internal class Publisher : IDisposable
    {
        protected static ITextMessage message = null;
        IConnection connection;
        ISession session;
        IMessageProducer producer;

        public void Dispose()
        {
            producer?.Dispose();
            session?.Dispose();
            connection?.Dispose();
        }

        public async Task Main(string host, int port, string queue = "weather")
        {
            Uri connecturi = new Uri($"activemq:tcp://{host}:{port}");

            Console.WriteLine("About to connect to " + connecturi);

            // NOTE: ensure the nmsprovider-activemq.config file exists in the executable folder.
            IConnectionFactory factory = new NMSConnectionFactory(connecturi);

            connection = factory.CreateConnection();
            session = connection.CreateSession();
            IDestination destination = SessionUtil.GetDestination(session, $"topic://{queue}");

            Console.WriteLine("Using destination: " + destination);

            // Create a consumer and producer
            producer = session.CreateProducer(destination);
            // Start the connection so that messages will be processed.
            connection.Start();
            // PERSIST - сохранять в БД, NON-PERSIST - не сохранять
            producer.DeliveryMode = MsgDeliveryMode.Persistent;
            producer.RequestTimeout = new TimeSpan(0, 0, 1);

            WeatherReader weatherReader = new WeatherReader();
            weatherReader.OnPublish += WeatherReader_OnPublish;
            await weatherReader.Start();
        }

        private void WeatherReader_OnPublish(IWeatherForecast value)
        {
            ITextMessage request = session.CreateTextMessage(ObjectHelper.ObjectToJson(value));
            request.NMSCorrelationID = "CityTemperature"; // так и не нашел для чего это
            request.Properties["temperature"] = value.TypeTemperature;
            request.Properties["city"] = value.City;
            
            //request.NMSPriority  
            producer.Send(request);
        }
    }
}
