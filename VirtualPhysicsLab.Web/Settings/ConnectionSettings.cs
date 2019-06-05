using System;
using System.Linq;
using VirtualPhysicsLab.Web.Interfaces;

namespace VirtualPhysicsLab.Web.Settings
{
    public class ConnectionSettings : IConnectionSettings
    {
        public string ConnectionString { get; set; }
        public string OrganizationUrl => string.IsNullOrEmpty(ConnectionString) ? null : ExtractOrganizationUrl(ConnectionString);
        public string DbConnectionString { get; set; }

        public ConnectionSettings()
        {

        }

        public ConnectionSettings(string connectionString)
        {
            ConnectionString = connectionString;
        }

        private string ExtractOrganizationUrl(string crmConnectionString)
        {
            var keys = crmConnectionString.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split('='))
                .ToDictionary(x => x[0].ToLower(), x => x[1].ToLower());

            keys.TryGetValue("url", out string url);

            if (url == null)
            {
                throw new Exception("Invalid VPL Organization URL in ConnectionString");
            }
            else
            {
                var uri = new Uri(url);

                return $"https://{uri.Host}/";
            }
        }
    }
}
