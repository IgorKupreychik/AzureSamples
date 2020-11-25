using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            const string queueName = "igkServiceBusQueue";

            var containersList = new List<Container>
            {
                new Container
                {
                    Id = 1,
                    Title = "First"
                },
                new Container
                {
                    Id = 2,
                    Title = "Second"
                },
                new Container
                {
                    Id = 3,
                    Title = "Third"
                }
            };

            var client = new MyServiceBusClient();

            foreach (var obj in containersList)
            {
                await client.SendMessageAsync(queueName, obj);
            }


            while (await client.MessageCount(queueName) > 0)
            {
                ServiceBusReceivedMessage message = await client.ReceiveMessageAsync(queueName);

                Container container = JsonConvert.DeserializeObject<Container>(Encoding.UTF8.GetString(message.Body));
            }
        }
    }
}
