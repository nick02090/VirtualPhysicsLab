﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VirtualPhysicsLab.Data.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<IEnumerable<T>> GetAsync();
        Task<T> GetAsync(Guid id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
