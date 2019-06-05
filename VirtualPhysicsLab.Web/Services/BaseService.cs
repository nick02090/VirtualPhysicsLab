using VirtualPhysicsLab.Web.Interfaces;

namespace VirtualPhysicsLab.Web.Services
{
    public abstract class BaseService
    {
        public readonly IConnectionSettings ConnectionSettings;
        public BaseService (IConnectionSettings connectionSettings)
        {
            ConnectionSettings = connectionSettings;
        }
    }
}
