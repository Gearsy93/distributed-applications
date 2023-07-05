using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace QueueSocket
{
    public class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                HostName = "192.168.0.9",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            using (var connection = factory.CreateConnection()) 
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "theatr-queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, e) =>
                {
                    var body = e.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine(message);
                };

                channel.CallbackException += (chann, args) =>
                {
                    Console.WriteLine(args.Exception.Message);
                };

                channel.BasicConsume(queue: "theatr-queue",
                                autoAck: false,
                                consumer: consumer);

                Console.WriteLine("Subscribed to the queue " + "theatr-queue");
            }
        }
    }
}