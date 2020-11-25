using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace QueueStorage
{
    public class MyQueueClient
    {
        //-------------------------------------------------
        // Create the queue service client
        //-------------------------------------------------
        public void CreateQueueClient(string queueName)
        {
            // Get the connection string from app settings
            var connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            var queueClient = new QueueClient(connectionString, queueName);
        }


        //-------------------------------------------------
        // Create a message queue
        //-------------------------------------------------
        public bool CreateQueue(string queueName)
        {
            try
            {
                // Get the connection string from app settings
                var connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

                // Instantiate a QueueClient which will be used to create and manipulate the queue
                var queueClient = new QueueClient(connectionString, queueName);

                // Create the queue
                queueClient.CreateIfNotExists();

                if (queueClient.Exists())
                {
                    Console.WriteLine($"Queue created: '{queueClient.Name}'");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Make sure the Azurite storage emulator running and try again.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}\n\n");
                Console.WriteLine($"Make sure the Azurite storage emulator running and try again.");
                return false;
            }
        }

        public void DeleteQueue(string queueName)
        {
            // Get the connection string from app settings
            string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

            // Instantiate a QueueClient which will be used to manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            if (queueClient.Exists())
            {
                // Delete the queue
                queueClient.Delete();
            }

            Console.WriteLine($"Queue deleted: '{queueClient.Name}'");
        }

        //-------------------------------------------------
        // Insert a message into a queue
        //-------------------------------------------------
        public void InsertTextMessage(string queueName, string message)
        {
            // Get the connection string from app settings
            var connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            var queueClient = new QueueClient(connectionString, queueName);

            // Create the queue if it doesn't already exist
            queueClient.CreateIfNotExists();

            if (queueClient.Exists())
            {
                // Send a message to the queue
                queueClient.SendMessage(message);
            }

            Console.WriteLine($"Inserted: {message}");
        }

        public void InsertBinaryMessage(string queueName, object message)
        {
            // Get the connection string from app settings
            var connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            var queueClient = new QueueClient(connectionString, queueName);

            // Create the queue if it doesn't already exist
            queueClient.CreateIfNotExists();

            if (queueClient.Exists())
            {
                // Send a message to the queue
                //var data = new BinaryData(message.ToByteArray());
                //var data = message as BinaryData;
                var data = Convert.ToBase64String(message.ToByteArray());
                queueClient.SendMessage(data);
            }

            Console.WriteLine("Inserted binary");
        }

        public QueueMessage GetMessage(string queueName)
        {
            // Get the connection string from app settings
            var connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            var queueClient = new QueueClient(connectionString, queueName);

            // Create the queue if it doesn't already exist
            queueClient.CreateIfNotExists();

            if (queueClient.Exists())
            {
                // Send a message to the queue
                var response = queueClient.ReceiveMessage();
                return response.Value;
            }

            return null;
        }


        public void DequeueMessage(string queueName, QueueMessage message)
        {
            // Get the connection string from app settings
            string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

            // Instantiate a QueueClient which will be used to manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            if (queueClient.Exists())
            {
                // Delete the message
                queueClient.DeleteMessage(message.MessageId, message.PopReceipt);
            }
        }


        //-----------------------------------------------------
        // Get the approximate number of messages in the queue
        //-----------------------------------------------------
        public int GetQueueLength(string queueName)
        {
            // Get the connection string from app settings
            string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

            // Instantiate a QueueClient which will be used to manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            if (queueClient.Exists())
            {
                QueueProperties properties = queueClient.GetProperties();

                // Retrieve the cached approximate message count.
                int cachedMessagesCount = properties.ApproximateMessagesCount;

                // Display number of messages.
                Console.WriteLine($"Number of messages in queue: {cachedMessagesCount}");

                return cachedMessagesCount;
            }

            return 0;
        }
    }
}
