using System;
using System.Threading.Tasks;
using VirtualPhysicsLab.Web.Interfaces;
using VirtualPhysicsLab.Web.Models;

namespace VirtualPhysicsLab.Web.Services
{
    public class ExperimentService : IExperimentService
    {
        private readonly IExperimentRepository ExperimentRepository;
        private readonly IMeshRepository MeshRepository;
        private readonly IExperimentSettingsRepository ExperimentSettingsRepository;

        public ExperimentService(IExperimentRepository experimentRepository, IMeshRepository meshRepository, IExperimentSettingsRepository experimentSettingsRepository)
        {
            ExperimentRepository = experimentRepository;
            MeshRepository = meshRepository;
            ExperimentSettingsRepository = experimentSettingsRepository;
        }

        public async Task<bool> CheckAvailability(string title, Guid userId)
        {
            var experiment = await ExperimentRepository.GetByUserAndTitleAsync(title, userId);

            if (experiment == null)
            {
                return true;
            }

            return false;
        }

        public async Task<ExperimentSettings> CreateSettingsAsync(ExperimentSettings entity)
        {
            var experimentSettings = await ExperimentSettingsRepository.CreateAsync(entity);

            return experimentSettings;
        }

        public async Task<ExperimentSettings> UpdateSettingsAsync(ExperimentSettings entity)
        {
            var newExperimentSettings = await ExperimentSettingsRepository.GetByExperimentAsync(entity.ExperimentId);
            newExperimentSettings.Axis = entity.Axis;
            newExperimentSettings.Friction = entity.Friction;
            newExperimentSettings.Restitution = entity.Restitution;
            newExperimentSettings.Walls = entity.Walls;

            var experimentSettings = await ExperimentSettingsRepository.UpdateAsync(newExperimentSettings);

            return experimentSettings;
        }
    }
}
