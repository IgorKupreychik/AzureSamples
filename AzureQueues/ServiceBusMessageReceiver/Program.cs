using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBusMessageReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
            var queueName = "igkServiceBusQueue";
            var queueClient = new QueueClient(connectionString, queueName);
            var messageHandlerOptions = new MessageHandlerOptions(OnException);
            queueClient.RegisterMessageHandler(OnMessage, messageHandlerOptions);
            Console.WriteLine("Listening, press any key");
            Console.ReadKey();
        }

        static Task OnMessage(Message message, CancellationToken ct)
        {
            Container container = JsonConvert.DeserializeObject<Container>(Encoding.UTF8.GetString(message.Body));
            Console.WriteLine($"Received messsage with ID '{message.MessageId}'. Enqueued at {message.SystemProperties.EnqueuedTimeUtc}");
            return Task.CompletedTask;
        }

        static Task OnException(ExceptionReceivedEventArgs args)
        {
            Console.WriteLine("Got an exception:");
            Console.WriteLine(args.Exception.Message);
            Console.WriteLine(args.ExceptionReceivedContext.ToString());
            return Task.CompletedTask;
        }
    }
}
