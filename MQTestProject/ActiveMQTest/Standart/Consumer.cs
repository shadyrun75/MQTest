using Apache.NMS;
using Apache.NMS.Util;
using MQTestProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveMQTest.Standart
{
    internal class Consumer
    {
        protected static AutoResetEvent semaphore = new AutoResetEvent(false);
        public async Task Main(string host, int port, string queue = "weather")
        {
            Console.Clear();
            var settings = new ConsumerSettings();
            settings.Init();
            Uri connecturi = new Uri($"activemq:tcp://{host}:{port}");

            Console.WriteLine("About to connect to " + connecturi);

            // NOTE: ensure the nmsprovider-activemq.config file exists in the executable folder.
            IConnectionFactory factory = new NMSConnectionFactory(connecturi);

            //using (IConnection connection = factory.CreateConnection(userName, password))
            using (IConnection connection = factory.CreateConnection())
            {
                connection.ClientId = "MQTestProject";  
                using (ISession session = connection.CreateSession())
                {
                    //IDestination destination = SessionUtil.GetDestination(session, $"topic://{queue}");
                    ITopic destination = SessionUtil.GetTopic(session, $"topic://{queue}");

                    //Debug.Assert(destination is ITopic);

                    // Consumer потребляет сообщения в один поток. Надо проверить в RabbitMQ

                    Console.WriteLine("Using destination: " + destination);

                    // Create a consumer and producer
                    string rk = GenerateRK(settings);

                    // DURABLE - если вкл, то сообщения будут сообщаться всем подписчикам, даже если они были отключены во время отправки сообщения из брокера
                    // иначе сообщения будут доставляться только подписчикам, которые были онлайн

                    //using (IMessageConsumer consumer = session.CreateConsumer(destination, rk))
                    using (IMessageConsumer consumer = session.CreateDurableConsumer(destination, GenerateDurableConsumerName(settings, queue), rk, true))
                    {
                        // Start the connection so that messages will be processed.
                        connection.Start();
                        consumer.Listener += new MessageListener(OnMessage);
                        await ConsumerWaiter.Run();
                    }
                }
            }
            Console.WriteLine("Exit from consumer");
        }

        protected static void OnMessage(IMessage receivedMsg)
        {
            var message = receivedMsg as ITextMessage;
            if (message == null)
            {
                Console.WriteLine("No message received!");
            }
            else
            {
                Console.WriteLine("Received message with ID:   " + message.NMSMessageId);
                Console.WriteLine("Received message with NMSCorrelationID:   " + message.NMSCorrelationID);
                Console.WriteLine("Received message with text: " + message.Text);
            }
            semaphore.Set();
        }

        string GenerateRK(ConsumerSettings value)
        {
            List<string> result = new List<string>();
            if (value.City?.Length > 0)
                result.Add($"city='{value.City}'");
            if (value.Temperature?.Length > 0)
                result.Add($"temperature='{value.Temperature}'");
            return String.Join(" AND ", result);
        }

        string GenerateDurableConsumerName(ConsumerSettings value, string queue)
        {
            string name = queue;
            if (value.City?.Length > 0)
                name += $"_{value.City}";
            if (value.Temperature?.Length > 0)
                name += $"_{value.Temperature}";
            return name;
        }
    }
}
