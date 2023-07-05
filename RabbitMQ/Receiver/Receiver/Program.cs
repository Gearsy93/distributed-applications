
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Receiver
{
    public class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();

            connectionFactory.Port = 5672;
            connectionFactory.HostName = "localhost";
            connectionFactory.UserName = "guest";
            connectionFactory.Password = "guest";
            connectionFactory.VirtualHost = "/";

            IConnection connection = connectionFactory.CreateConnection();
            IModel channel = connection.CreateModel();
            //channel.BasicQos(0, 1, false);

            channel.QueueDeclare(queue: "theatr-queue",
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
            EventingBasicConsumer eventingBasicConsumer = new EventingBasicConsumer(channel);

            eventingBasicConsumer.Received += (sender, e) =>
            {
                var body = e.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine(" Reveived message: {0}", message);
            };

            eventingBasicConsumer.Received += (sender, basicDeliveryEventArgs) =>
            {
                string message = Encoding.UTF8.GetString(basicDeliveryEventArgs.Body.ToArray());
                channel.BasicAck(basicDeliveryEventArgs.DeliveryTag, false);
                Console.WriteLine("Message: {0} {1}", message, " Enter your response: ");
                string response = Console.ReadLine();
                IBasicProperties replyBasicProperties = channel.CreateBasicProperties();
                replyBasicProperties.CorrelationId = basicDeliveryEventArgs.BasicProperties.CorrelationId;
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                channel.BasicPublish("", basicDeliveryEventArgs.BasicProperties.ReplyTo, replyBasicProperties, responseBytes);
            };

            channel.BasicConsume("theatr-queue", false, eventingBasicConsumer);
        }
    }
    
}