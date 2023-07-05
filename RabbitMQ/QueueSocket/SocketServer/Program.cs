using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SocketServer
{
    public class Program
    {
        private static string ServerCertificateName = ConfigSocket.CertName;
        static void Main(string[] args)
        {
            var serverCertificate = getServerCert();
            TcpListener Server = new TcpListener(IPAddress.Any, ConfigSocket.Port);
            Server.Start();
            Console.WriteLine("Server started on port " + ConfigSocket.Port);
            using (var client = Server.AcceptTcpClient())
            using (var sslStream = new SslStream(client.GetStream(), false, ValidateCertificate))
            {
                sslStream.AuthenticateAsServer(serverCertificate,
                   true, SslProtocols.Tls12, false);

                Console.WriteLine("SSL stream established");
                bool isover = false;
                while (!isover)
                {
                    var inputBuffer = new byte[4096];
                    var inputBytes = 0;
                    var inputMessage = "";
                    while (inputBytes == 0)
                    {
                        inputBytes = sslStream.Read(inputBuffer, 0, inputBuffer.Length);
                        inputMessage = Encoding.UTF8.GetString(inputBuffer, 0, inputBytes);
                        if (inputMessage == "Over")
                        {
                            Console.WriteLine("Got over");
                            inputBytes = 0;
                            isover = true;
                            break;
                        }
                        sslStream.Write(Encoding.UTF8.GetBytes("+"));

                    }
                    Console.WriteLine("GOT Data: {0}", inputMessage);

                    if (inputMessage != "+" && inputMessage != "Over")
                    {
                        var dbUpdate = new DBUpdate.DBUpdate(inputMessage);
                        dbUpdate.Insert_String();
                    }
                }
            }
        }

        static bool ValidateCertificate(Object sender,X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
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
