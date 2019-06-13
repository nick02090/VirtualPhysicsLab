using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualPhysicsLab.Data.Models;
using VirtualPhysicsLab.Data.Repositories;
using VirtualPhysicsLab.Web.Data;
using VirtualPhysicsLab.Web.Interfaces;
using VirtualPhysicsLab.Web.Models;

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
            return await VPLContext.Experiments
                .Select(x => new Experiment
                {
                    CreatedBy = new User
                    {
                        Id = x.CreatedBy.Id,
                        FirstName = x.CreatedBy.FirstName,
                        LastName = x.CreatedBy.LastName
                    },
                    Id = x.Id,
                    CreatedOn = x.CreatedOn,
                    Description = x.Description,
                    LogicalName = x.LogicalName,
                    Name = x.Name,
                    Title = x.Title,
                    Picture = x.Picture
                }).ToArrayAsync();
        }

        public async Task<Experiment> GetAsync(Guid id)
        {
            return await VPLContext.Experiments
                .Include(x => x.Settings)
                .Select(x => new Experiment
                {
                    CreatedBy = new User
                    {
                        Id = x.CreatedBy.Id
                    },
                    Settings = new ExperimentSettings
                    {
                        Gravity = x.Settings.Gravity,
                        Axis = x.Settings.Axis,
                        Friction = x.Settings.Friction,
                        Restitution = x.Settings.Restitution,
                        Walls = x.Settings.Walls,
                        Id = x.Settings.Id,
                        CreatedOn = x.Settings.CreatedOn,
                        LogicalName = x.Settings.LogicalName,
                        Size = x.Settings.Size
                    },
                    CreatedOn = x.CreatedOn,
                    Description = x.Description,
                    Id = x.Id,
                    LogicalName = x.LogicalName,
                    Name = x.Name,
                    Title = x.Title,
                    Picture = x.Picture
                }).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Experiment> GetByIdAsync(Guid id)
        {
            return await VPLContext.Experiments.FindAsync(id);
        }

        public async Task<Experiment> GetByUserAndTitleAsync(string title, Guid userId)
        {
            return await VPLContext.Experiments.Where(x => x.CreatedBy.Id == userId && x.Title == title).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Experiment>> GetByUserAsync(Guid id)
        {
            return await VPLContext.Experiments.Where(x => x.CreatedBy.Id == id)
                .Include(x => x.Settings)
                .Select(x => new Experiment
                {
                    CreatedBy = new User
                    {
                        Id = x.CreatedBy.Id
                    },
                    Settings = new ExperimentSettings
                    {
                        Gravity = x.Settings.Gravity,
                        Axis = x.Settings.Axis,
                        Friction = x.Settings.Friction,
                        Restitution = x.Settings.Restitution,
                        Walls = x.Settings.Walls,
                        Id = x.Settings.Id,
                        CreatedOn = x.Settings.CreatedOn,
                        LogicalName = x.Settings.LogicalName,
                        Size = x.Settings.Size
                    },
                    CreatedOn = x.CreatedOn,
                    Description = x.Description,
                    Id = x.Id,
                    LogicalName = x.LogicalName,
                    Name = x.Name,
                    Title = x.Title,
                    Picture = x.Picture
                }).ToArrayAsync();
        }

        public async Task<Experiment> UpdateAsync(Experiment entity)
        {
            var experiment = await VPLContext.Experiments
                .Include(x => x.Meshes)
                .Include(x => x.Settings)
                .Include(x => x.CreatedBy)
                .Where(x => x.Id == entity.Id).SingleOrDefaultAsync();

            experiment.Description = entity.Description;
            experiment.Title = entity.Title;
            experiment.Name = entity.Title;
            experiment.Picture = entity.Picture;

            VPLContext.Entry(experiment).State = EntityState.Modified;

            try
            {
                await VPLContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExperimentExists(experiment.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return experiment;
        }
        private bool ExperimentExists(Guid id)
        {
            return VPLContext.Experiments.Any(e => e.Id == id);
        }
    }
}
