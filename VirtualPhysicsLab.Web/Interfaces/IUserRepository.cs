using System.Threading.Tasks;
using VirtualPhysicsLab.Data.Interfaces;
using VirtualPhysicsLab.Data.Models;

namespace VirtualPhysicsLab.Web.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByNicknameAsync(string nickname);
        Task<User> GetByEmailAsync(string email);
    }
}
