using System;

namespace QueueStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            const string queueName = "testqueue";

            var client = new MyQueueClient();

            client.CreateQueue(queueName);

            client.InsertTextMessage(queueName, "Hello world!!!!");

            client.InsertBinaryMessage(queueName, new Container { Id = 1, Title = "First element" });

            while (client.GetQueueLength(queueName) > 0)
            {
                var queueElement = client.GetMessage(queueName);

                Console.WriteLine($"Received: {queueElement.Body}");

                client.DequeueMessage(queueName, queueElement);
            }




            client.DeleteQueue(queueName);
        }
    }
}
