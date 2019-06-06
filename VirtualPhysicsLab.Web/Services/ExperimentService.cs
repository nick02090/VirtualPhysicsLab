using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualPhysicsLab.Data.Models;
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

        public async Task<ExperimentSettings> CreateSettingsAsync(ExperimentSettings entity)
        {
            var experimentSettings = await ExperimentSettingsRepository.CreateAsync(entity);

            return experimentSettings;
        }
    }
}
