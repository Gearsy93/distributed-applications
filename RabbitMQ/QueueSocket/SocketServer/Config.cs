using System.Configuration;

namespace SocketServer
{
    public static class ConfigSocket
    {
        public static string Host = ConfigurationSettings.AppSettings["Host"];
        public static int Port = int.Parse(ConfigurationSettings.AppSettings["Port"]);
        public static string CertName = ConfigurationSettings.AppSettings["CertName"];
        public static string PGHost = ConfigurationSettings.AppSettings["PGHost"];
        public static int PGPort = int.Parse(ConfigurationSettings.AppSettings["PGPort"]);
        public static string Database = ConfigurationSettings.AppSettings["PGDatabase"];
        public static string Username = ConfigurationSettings.AppSettings["PGUsername"];
        public static string Password = ConfigurationSettings.AppSettings["PGPassword"];
    }
}
