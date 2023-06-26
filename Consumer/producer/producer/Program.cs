using RabbitMQ.Client;
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

                // Start the producer
                Console.WriteLine("Producer Microservice - Enter a message to send (or 'exit' to quit):");

                while (true)
                {
                    string input = Console.ReadLine();

                    if (input.ToLower() == "exit")
                        break;

                    var body = Encoding.UTF8.GetBytes(input);

                    channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

                    Console.WriteLine("Sent message: {0}", input);
                }
            }
        }
    }
}
