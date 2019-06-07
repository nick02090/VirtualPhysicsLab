using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtualPhysicsLab.Data.Interfaces;
using VirtualPhysicsLab.Data.Models;

namespace VirtualPhysicsLab.Web.Interfaces
{
    public interface IMeshRepository : IBaseRepository<Mesh>
    {
        Task<IEnumerable<Mesh>> GetByExperimentAsync(Guid id);
    }
}
