using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualPhysicsLab.Data.Models;
using VirtualPhysicsLab.Data.Repositories;
using VirtualPhysicsLab.Web.Data;
using VirtualPhysicsLab.Web.Interfaces;

namespace VirtualPhysicsLab.Web.Repositories
{
    public class ExperimentRepository : BaseRepository, IExperimentRepository
    {
        public ExperimentRepository(VPLContext context) : base(context)
        {
        }

        public async Task<Experiment> CreateAsync(Experiment entity)
        {
            VPLContext.Experiments.Add(entity);
            await VPLContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var experiment = await VPLContext.Experiments.FindAsync(id);
            if (experiment == null)
            {
                return;
            }

            VPLContext.Experiments.Remove(experiment);
            await VPLContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Experiment>> GetAsync()
        {
            return await VPLContext.Experiments.Include(x => x.CreatedBy).ToArrayAsync();
        }

        public async Task<Experiment> GetAsync(Guid id)
        {
            return await VPLContext.Experiments.FindAsync(id);
        }

        public async Task<IEnumerable<Experiment>> GetByUserAsync(Guid id)
        {
            return await VPLContext.Experiments.Where(x => x.CreatedBy.Id == id).ToArrayAsync();
        }

        public async Task<Experiment> UpdateAsync(Experiment entity)
        {
            VPLContext.Entry(entity).State = EntityState.Modified;

            try
            {
                await VPLContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExperimentExists(entity.Id))
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
        private bool ExperimentExists(Guid id)
        {
            return VPLContext.Experiments.Any(e => e.Id == id);
        }
    }
}
