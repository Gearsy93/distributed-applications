using System.Configuration;

namespace SocketClient
{
    public static class ConfigSocket
    {
        public static string Host = ConfigurationSettings.AppSettings["host"];
        public static int Port = int.Parse(ConfigurationSettings.AppSettings["port"]);
        public static string CertName = ConfigurationSettings.AppSettings["CertName"];
    }
}
