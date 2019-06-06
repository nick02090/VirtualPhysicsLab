using System.Threading.Tasks;
using VirtualPhysicsLab.Data.Models;
using VirtualPhysicsLab.Web.Models;

namespace VirtualPhysicsLab.Web.Interfaces
{
    public interface IExperimentService
    {
        Task<ExperimentSettings> CreateSettingsAsync(ExperimentSettings experimentSettings);
    }
}
