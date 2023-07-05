using System.Configuration;

namespace DBUpdate
{
    public static class ConfigDB
    {
        public static string Host = ConfigurationSettings.AppSettings["PGHost"];
        public static int Port = int.Parse(ConfigurationSettings.AppSettings["PGPort"]);
        public static string Database = ConfigurationSettings.AppSettings["PGDatabase"];
        public static string Username = ConfigurationSettings.AppSettings["PGUsername"];
        public static string Password = ConfigurationSettings.AppSettings["PGPassword"];
    }
}
