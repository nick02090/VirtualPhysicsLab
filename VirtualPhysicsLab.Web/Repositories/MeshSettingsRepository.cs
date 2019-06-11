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
    public class MeshSettingsRepository : BaseRepository, IMeshSettingsRepository
    {
        public MeshSettingsRepository(VPLContext context) : base(context)
        {
        }

        public async Task<MeshSettings> CreateAsync(MeshSettings entity)
        {
            VPLContext.MeshSettings.Add(entity);
            await VPLContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var meshSettings = await VPLContext.MeshSettings.FindAsync(id);
            if (meshSettings == null)
            {
                return;
            }

            VPLContext.MeshSettings.Remove(meshSettings);
            await VPLContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<MeshSettings>> GetAsync()
        {
            return await VPLContext.MeshSettings.ToArrayAsync();
        }

        public async Task<MeshSettings> GetAsync(Guid id)
        {
            return await VPLContext.MeshSettings.FindAsync(id);
        }

        public async Task<MeshSettings> GetByMeshAsync(Guid meshId)
        {
            return await VPLContext.MeshSettings
                .Include(x => x.Mesh)
                .Include(x => x.Color)
                .Include(x => x.Position)
                .Include(x => x.Rotation)
                .Include(x => x.Size)
                .Include(x => x.Velocity)
                .Where(x => x.MeshId == meshId).SingleOrDefaultAsync();
        }

        public async Task<MeshSettings> UpdateAsync(MeshSettings entity)
        {
            VPLContext.Entry(entity).State = EntityState.Modified;

            try
            {
                await VPLContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeshSettingsExists(entity.Id))
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
        private bool MeshSettingsExists(Guid id)
        {
            return VPLContext.MeshSettings.Any(e => e.Id == id);
        }
    }
}
