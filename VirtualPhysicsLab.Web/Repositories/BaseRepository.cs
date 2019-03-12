using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtualPhysicsLab.Data.Interfaces;
using VirtualPhysicsLab.Web.Data;

namespace VirtualPhysicsLab.Data.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T>
    {
        protected VPLContext VPLContext { get; }
        public BaseRepository(VPLContext context)
        {
            VPLContext = context;
        }

        public abstract Task<IEnumerable<T>> GetAsync();
        public abstract Task<T> GetAsync(Guid id);
        public abstract Task<T> CreateAsync(T entity);
        public abstract Task<T> UpdateAsync(T entity);
        public abstract Task DeleteAsync(Guid id);
    }
}
