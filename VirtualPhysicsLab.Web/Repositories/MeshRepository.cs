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
    public class MeshRepository : BaseRepository, IMeshRepository
    {
        public MeshRepository(VPLContext context) : base(context)
        {
        }

        public async Task<Mesh> CreateAsync(Mesh entity)
        {
            VPLContext.Meshes.Add(entity);
            await VPLContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var mesh = await VPLContext.Meshes.FindAsync(id);
            if (mesh == null)
            {
                return;
            }

            VPLContext.Meshes.Remove(mesh);
            await VPLContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Mesh>> GetAsync()
        {
            return await VPLContext.Meshes.ToArrayAsync();
        }

        public async Task<Mesh> GetAsync(Guid id)
        {
            return await VPLContext.Meshes.FindAsync(id);
        }

        public async Task<Mesh> UpdateAsync(Mesh entity)
        {
            VPLContext.Entry(entity).State = EntityState.Modified;

            try
            {
                await VPLContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeshExists(entity.Id))
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

        private bool MeshExists(Guid id)
        {
            return VPLContext.Meshes.Any(e => e.Id == id);
        }
    }
}
