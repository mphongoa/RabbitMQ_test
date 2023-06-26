using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        // RabbitMQ connection settings
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };

        // Create a connection to RabbitMQ
        using (var connection = factory.CreateConnection())
        {
            // Create a channel
            using (var channel = connection.CreateModel())
            {
               string queueName = "myQueue";

             

                // Declare the queue
                channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                // Start the consumer
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("Consumer Microservice - Received message: {0}", message);
                };

                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                Console.WriteLine("Consumer Microservice - Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
