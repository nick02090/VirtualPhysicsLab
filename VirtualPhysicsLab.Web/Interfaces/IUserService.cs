using System.Threading.Tasks;
using VirtualPhysicsLab.Data.Models;

namespace VirtualPhysicsLab.Web.Interfaces
{
    public interface IUserService
    {
        Task<User> Authenticate(string email, string password);
        Task<bool> CheckAvailability(string email);
        Task Logout(string token);
    }
}
