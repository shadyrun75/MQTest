using Apache.NMS;
using Apache.NMS.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveMQTest
{
    internal class SimpleConsumer
    {
        protected static AutoResetEvent semaphore = new AutoResetEvent(false);
        public static void Main(string host, int port)
        {
            // Example connection strings:
            //    activemq:tcp://activemqhost:61616
            //    stomp:tcp://activemqhost:61613
            //    ems:tcp://tibcohost:7222
            //    msmq://localhost

            Uri connecturi = new Uri($"activemq:tcp://{host}:{port}");

            Console.WriteLine("About to connect to " + connecturi);

            // NOTE: ensure the nmsprovider-activemq.config file exists in the executable folder.
            IConnectionFactory factory = new NMSConnectionFactory(connecturi);

            using (IConnection connection = factory.CreateConnection())
            using (ISession session = connection.CreateSession())
            {
                // Examples for getting a destination:
                //
                // Hard coded destinations:
                //    IDestination destination = session.GetQueue("FOO.BAR");
                //    Debug.Assert(destination is IQueue);
                //    IDestination destination = session.GetTopic("FOO.BAR");
                //    Debug.Assert(destination is ITopic);
                //
                // Embedded destination type in the name:
                //    IDestination destination = SessionUtil.GetDestination(session, "queue://FOO.BAR");
                //    Debug.Assert(destination is IQueue);
                //    IDestination destination = SessionUtil.GetDestination(session, "topic://FOO.BAR");
                //    Debug.Assert(destination is ITopic);
                //
                // Defaults to queue if type is not specified:
                //    IDestination destination = SessionUtil.GetDestination(session, "FOO.BAR");
                //    Debug.Assert(destination is IQueue);
                //
                // .NET 3.5 Supports Extension methods for a simplified syntax:
                //    IDestination destination = session.GetDestination("queue://FOO.BAR");
                //    Debug.Assert(destination is IQueue);
                //    IDestination destination = session.GetDestination("topic://FOO.BAR");
                //    Debug.Assert(destination is ITopic);
                //IDestination destination = SessionUtil.GetDestination(session, "queue://FOO.BAR");

                IDestination destination = SessionUtil.GetDestination(session, "topic://FOO.BAR");
                Debug.Assert(destination is ITopic);

                Console.WriteLine("Using destination: " + destination);

                // Create a consumer and producer
                using (IMessageConsumer consumer = session.CreateConsumer(destination))
                {
                    // Start the connection so that messages will be processed.
                    connection.Start();
                    
                    consumer.Listener += new MessageListener(OnMessage);
                    Task.Run(Exit);
                    while (true)
                    {
                        // Wait for the message
                        semaphore.WaitOne();                       
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
                foreach (var item in message.Properties.Keys)
                    Console.WriteLine("Received message with Keys:   " + item);
                Console.WriteLine("Received message with text: " + message.Text);
            }
            semaphore.Set();
        }

        private static async Task Exit()
        {
            try
            {
                Console.WriteLine("Press enter to stop server");

                await Task.Run(() => Console.ReadLine());
                semaphore.Close();
            }
            finally
            {
                
            }
        }
    }
}
