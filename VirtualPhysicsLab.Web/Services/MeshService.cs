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
        public async Task<MeshSettings> UpdateSettingsAsync(MeshSettings entity)
        {
            var newMeshSettings = await MeshSettingsRepository.GetByMeshAsync(entity.MeshId);
            newMeshSettings.Axis = entity.Axis;
            newMeshSettings.Friction = entity.Friction;
            newMeshSettings.Restitution = entity.Restitution;
            newMeshSettings.Color = entity.Color;
            newMeshSettings.Mass = entity.Mass;
            newMeshSettings.Position = entity.Position;
            newMeshSettings.Rotation = entity.Rotation;
            newMeshSettings.Size = entity.Size;
            newMeshSettings.Velocity = entity.Velocity;

            var experimentSettings = await MeshSettingsRepository.UpdateAsync(newMeshSettings);

            return experimentSettings;
        }
    }
}
