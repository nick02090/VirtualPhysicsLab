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
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(VPLContext context) : base(context)
        {
        }

        public async Task<User> CreateAsync(User entity)
        {
            VPLContext.Users.Add(entity);
            await VPLContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await VPLContext.Users.FindAsync(id);
            if (user == null)
            {
                return;
            }

            VPLContext.Users.Remove(user);
            await VPLContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            return await VPLContext.Users.ToArrayAsync();
        }

        public async Task<User> GetAsync(Guid id)
        {
            return await VPLContext.Users.FindAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await VPLContext.Users.Where(x => x.Email.Equals(email)).SingleOrDefaultAsync();
        }

        public async Task<User> GetByTokenAsync(string token)
        {
            return await VPLContext.Users.Where(x => x.Token.Equals(token)).SingleOrDefaultAsync();
        }

        public async Task<User> UpdateAsync(User entity)
        {
            VPLContext.Entry(entity).State = EntityState.Modified;

            try
            {
                await VPLContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(entity.Id))
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

        private bool UserExists(Guid id)
        {
            return VPLContext.Users.Any(e => e.Id == id);
        }
    }
}
