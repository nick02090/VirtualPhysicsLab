using System;
using System.Threading.Tasks;
using VirtualPhysicsLab.Data.Models;
using VirtualPhysicsLab.Web.Models;

namespace VirtualPhysicsLab.Web.Interfaces
{
    public interface IExperimentService
    {
        Task<ExperimentSettings> CreateSettingsAsync(ExperimentSettings experimentSettings);
        Task<bool> CheckAvailability(string title, Guid userId);
        Task<ExperimentSettings> UpdateSettingsAsync(ExperimentSettings experimentSettings);
    }
}
