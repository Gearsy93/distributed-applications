using System.Configuration;

namespace QueueSocket
{

    public static class Config
    {
        public static string host = ConfigurationSettings.AppSettings["host"];
        public static int port = int.Parse(ConfigurationSettings.AppSettings["port"]);
        public static string UserName = ConfigurationSettings.AppSettings["UserName"];
        public static string Password = ConfigurationSettings.AppSettings["Password"];
        public static string ServerName = ConfigurationSettings.AppSettings["ServerName"];
        public static string CertPath = ConfigurationSettings.AppSettings["CertPath"];
        public static string CertPassphrase = ConfigurationSettings.AppSettings["CertPassphrase"];
        public static string CertName = ConfigurationSettings.AppSettings["CertName"];
    }
}