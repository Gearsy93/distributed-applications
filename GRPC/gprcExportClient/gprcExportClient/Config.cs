using System.Configuration;

namespace gprcExportClient
{
    public static class Config
    {
        public static string Host = ConfigurationSettings.AppSettings["Host"];
        public static string Port = ConfigurationSettings.AppSettings["Port"];
    }
}
