using System.Configuration;

namespace ImportService
{
    public static class Config
    {
        public static string Host = ConfigurationSettings.AppSettings["PGHost"];
        public static int Port = int.Parse(ConfigurationSettings.AppSettings["PGPort"]);
        public static string Database = ConfigurationSettings.AppSettings["PGDatabase"];
        public static string Username = ConfigurationSettings.AppSettings["PGUsername"];
        public static string Password = ConfigurationSettings.AppSettings["PGPassword"];
    }
}
