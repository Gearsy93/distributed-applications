using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;


namespace SocketClient
{
    public class Program
    {
        private static string ServerCertificateName = ConfigSocket.CertName;
        static void Main(string[] args)
        {
            var clientCertificate = getServerCert();

            var clientCertificateCollection = new
               X509CertificateCollection(new X509Certificate[]
               { 
                   clientCertificate
               });

            using (var client = new TcpClient(ConfigSocket.Host, ConfigSocket.Port))
            using (var sslStream = new SslStream(client.GetStream(), false, ValidateCertificate))
            {
                sslStream.AuthenticateAsClient(ServerCertificateName,
                   clientCertificateCollection, SslProtocols.Tls12, false);
                Console.WriteLine("Connected to " + ConfigSocket.Host + ':' + ConfigSocket.Port);
                Console.WriteLine("SSL stream established");

                var outputMessage = "I haz secure data";
                var outputBuffer = Encoding.UTF8.GetBytes(outputMessage);
                sslStream.Write(outputBuffer);
                Console.WriteLine("Sent: {0}", outputMessage);
            }
        }

        static bool ValidateCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true; 
            }
            if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors)
            { 
                return true; 
            }
            return false;
        }

        private static X509Certificate getServerCert()
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2 foundCertificate = null;
            foreach (X509Certificate2 currentCertificate in store.Certificates)
            {
                if (currentCertificate.IssuerName.Name != null && currentCertificate.IssuerName.Name.Equals("CN=MySslSocketCertificate"))
                {
                    foundCertificate = currentCertificate;
                    break;
                }
            }

            return foundCertificate;
        }
    }
}