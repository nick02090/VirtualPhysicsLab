using System;
using System.Threading.Tasks;
using VirtualPhysicsLab.Data.Interfaces;
using VirtualPhysicsLab.Web.Models;

namespace VirtualPhysicsLab.Web.Interfaces
{
    public interface IExperimentSettingsRepository : IBaseRepository<ExperimentSettings>
    {
        Task<ExperimentSettings> GetByExperimentAsync(Guid experimentId);
    }
}
