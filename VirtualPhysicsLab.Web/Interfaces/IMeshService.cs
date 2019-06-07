using System.Threading.Tasks;
using VirtualPhysicsLab.Data.Models;

namespace VirtualPhysicsLab.Web.Interfaces
{
    public interface IMeshService
    {
        Task<MeshSettings> CreateSettingsAsync(MeshSettings meshSettings);
        Task<MeshSettings> UpdateSettingsAsync(MeshSettings entity);
    }
}
