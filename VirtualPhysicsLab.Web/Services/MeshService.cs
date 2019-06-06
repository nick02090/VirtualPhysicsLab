using System.Threading.Tasks;
using VirtualPhysicsLab.Data.Models;
using VirtualPhysicsLab.Web.Interfaces;

namespace VirtualPhysicsLab.Web.Services
{
    public class MeshService : IMeshService
    {
        private readonly IMeshSettingsRepository MeshSettingsRepository;
        public MeshService(IMeshSettingsRepository meshSettingsRepository)
        {
            MeshSettingsRepository = meshSettingsRepository;
        }
        public async Task<MeshSettings> CreateSettingsAsync(MeshSettings entity)
        {
            var meshSettings = await MeshSettingsRepository.CreateAsync(entity);

            return meshSettings;
        }
    }
}
