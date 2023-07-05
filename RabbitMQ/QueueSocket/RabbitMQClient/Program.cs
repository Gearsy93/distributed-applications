using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Security.Authentication;

namespace QueueSocket
{
    public class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                HostName = Config.host,
                Port = Config.port,
                UserName = Config.UserName,
                Password = Config.Password,
                Ssl = new SslOption
                {
                    Enabled = true,
                    ServerName = Config.ServerName,
                    CertPath = Config.CertPath,
                    CertPassphrase = Config.CertPassphrase,
                    Version = SslProtocols.Tls12,
                }
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
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
                var dbUpdate = new DBUpdate.DBUpdate(message);
                Console.WriteLine(message);
                dbUpdate.Insert_String();
            };

            channel.CallbackException += (chann, args) =>
            {
                Console.WriteLine(args.Exception.Message);
            };

            channel.BasicConsume(queue: "theatr-queue",
                                autoAck: true,
                                consumer: consumer);


            Console.WriteLine("Subscribed to the queue " + "theatr-queue");
            Console.ReadKey();
            channel.Dispose();
            connection.Close();
        }
    }
}