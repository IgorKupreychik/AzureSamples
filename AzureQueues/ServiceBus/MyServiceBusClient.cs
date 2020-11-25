using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus
{
    public class MyServiceBusClient
    {
        public async Task SendMessageAsync(string queueName, object obj)
        {
            string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

            // create a Service Bus client 
            await using (ServiceBusClient client = new ServiceBusClient(connectionString))
            {
                // create a sender for the queue 
                ServiceBusSender sender = client.CreateSender(queueName);


                string messageBody = JsonConvert.SerializeObject(obj);
                // create a message that we can send
                ServiceBusMessage message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody));

                // send the message
                await sender.SendMessageAsync(message);
                Console.WriteLine($"Sent a single message to the queue: {queueName}");
            }
        }

        public async Task<ServiceBusReceivedMessage> ReceiveMessageAsync(string queueName)
        {
            string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

            // create a Service Bus client 
            await using (ServiceBusClient client = new ServiceBusClient(connectionString))
            {
                // create a sender for the queue 
                ServiceBusReceiver receiver = client.CreateReceiver(queueName, 
                    new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete });

                ServiceBusReceivedMessage message = await receiver.ReceiveMessageAsync();

                return message;
            }
        }

        public async Task<long> MessageCount(string queueName)
        {
            string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
            var managementClient = new ManagementClient(connectionString);
            var queue = await managementClient.GetQueueRuntimeInfoAsync(queueName);
            var messageCount = queue.MessageCount;
            return messageCount;
        }
    }
}
