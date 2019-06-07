using System;
using System.Threading.Tasks;
using VirtualPhysicsLab.Data.Interfaces;
using VirtualPhysicsLab.Data.Models;

namespace VirtualPhysicsLab.Web.Interfaces
{
    public interface IMeshSettingsRepository : IBaseRepository<MeshSettings>
    {
        Task<MeshSettings> GetByMeshAsync(Guid meshId);
    }
}
