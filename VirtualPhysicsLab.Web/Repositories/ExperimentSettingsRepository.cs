using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualPhysicsLab.Data.Repositories;
using VirtualPhysicsLab.Web.Data;
using VirtualPhysicsLab.Web.Interfaces;
using VirtualPhysicsLab.Web.Models;

namespace VirtualPhysicsLab.Web.Repositories
{
    public class ExperimentSettingsRepository : BaseRepository, IExperimentSettingsRepository
    {
        public ExperimentSettingsRepository(VPLContext context) : base(context)
        {
        }

        public async Task<ExperimentSettings> CreateAsync(ExperimentSettings entity)
        {
            VPLContext.ExperimentSettings.Add(entity);
            await VPLContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var experimentSettings = await VPLContext.ExperimentSettings.FindAsync(id);
            if (experimentSettings == null)
            {
                return;
            }

            VPLContext.ExperimentSettings.Remove(experimentSettings);
            await VPLContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ExperimentSettings>> GetAsync()
        {
            return await VPLContext.ExperimentSettings.ToArrayAsync();
        }

        public async Task<ExperimentSettings> GetAsync(Guid id)
        {
            return await VPLContext.ExperimentSettings.FindAsync(id);
        }

        public async Task<ExperimentSettings> GetByExperimentAsync(Guid experimentId)
        {
            return await VPLContext.ExperimentSettings
                .Include(x => x.Experiment)
                .Include(x => x.Gravity)
                .Include(x => x.Size)
                .Where(x => x.ExperimentId == experimentId).SingleOrDefaultAsync();
        }

        public async Task<ExperimentSettings> UpdateAsync(ExperimentSettings entity)
        {
            VPLContext.Entry(entity).State = EntityState.Modified;

            try
            {
                await VPLContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExperimentSettingsExists(entity.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return entity;
        }
        private bool ExperimentSettingsExists(Guid id)
        {
            return VPLContext.ExperimentSettings.Any(e => e.Id == id);
        }
    }
}
