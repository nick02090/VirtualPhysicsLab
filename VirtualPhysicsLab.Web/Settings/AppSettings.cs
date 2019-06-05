using VirtualPhysicsLab.Web.Interfaces;

namespace VirtualPhysicsLab.Web.Settings
{
    public class AppSettings : IAppSettings
    {
        public string Secret { get; set; }
        public AppSettings()
        {

        }

        public AppSettings(string secret)
        {
            Secret = secret;
        }
    }
}
