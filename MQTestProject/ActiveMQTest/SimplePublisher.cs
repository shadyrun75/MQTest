using Apache.NMS;
using Apache.NMS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveMQTest
{
    internal class SimplePublisher
    {
        protected static AutoResetEvent semaphore = new AutoResetEvent(false);
        protected static ITextMessage message = null;
        protected static TimeSpan receiveTimeout = TimeSpan.FromSeconds(10);

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
                IDestination destination = SessionUtil.GetDestination(session, "topic://FOO.BAR");

                Console.WriteLine("Using destination: " + destination);

                // Create a consumer and producer
                using (IMessageProducer producer = session.CreateProducer(destination))
                {
                    // Start the connection so that messages will be processed.
                    connection.Start();
                    producer.DeliveryMode = MsgDeliveryMode.Persistent;
                    producer.RequestTimeout = receiveTimeout;

                    while (true)
                    {
                        // Send a message
                        var testMessage = GetMessage();
                        if (testMessage == "")
                            break;
                        ITextMessage request = session.CreateTextMessage(testMessage);
                        request.NMSCorrelationID = "abc";
                        request.Properties["NMSXGroupID"] = "cheese";
                        request.Properties["myHeader"] = "Cheddar";

                        producer.Send(request);
                    }
                }
            }
            Console.WriteLine("Exit from publisher");
        }

        static string GetMessage()
        {
            Console.WriteLine(" > ");
            return Console.ReadLine() ?? "";
        }
    }
}
