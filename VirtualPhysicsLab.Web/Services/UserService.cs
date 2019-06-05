using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VirtualPhysicsLab.Data.Models;
using VirtualPhysicsLab.Web.Helpers;
using VirtualPhysicsLab.Web.Interfaces;

namespace VirtualPhysicsLab.Web.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository UserRepository;
        private readonly IAppSettings AppSettings;

        public UserService(IUserRepository userRepository, IAppSettings appSettings)
        {
            UserRepository = userRepository;
            AppSettings = appSettings;
        }
        public async Task<User> Authenticate(string nickname, string password)
        {
            var user = await UserRepository.GetByNicknameAsync(nickname);

            if (user == null)
            {
                return null;
            }

            var passwordHasher = new PasswordHasher();
            if (!passwordHasher.VerifyHashedPassword(user.Password, password))
            {
                return null; // invalid password
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }
    }
}
