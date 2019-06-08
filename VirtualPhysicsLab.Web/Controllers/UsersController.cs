using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualPhysicsLab.Data.Models;
using VirtualPhysicsLab.Web.Helpers;
using VirtualPhysicsLab.Web.Interfaces;

namespace VirtualPhysicsLab.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly PasswordHasher _passwordHasher;
        public IUserService UserService { get; }
        public IUserRepository UserRepository { get; }
        public IConnectionSettings ConnectionSettings { get; }

        public UsersController(IUserService userService, IUserRepository userRepository, IConnectionSettings connectionSettings)
        {
            UserService = userService;
            UserRepository = userRepository;
            ConnectionSettings = connectionSettings;
            _passwordHasher = new PasswordHasher();
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IEnumerable<User>> GetUsers()
        {
            var result = await UserRepository.GetAsync();
            foreach (var r in result)
            {
                r.Password = null;
            }
            return result;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await UserRepository.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Password = null;

            return Ok(user);
        }
        
        [HttpGet("token")]
        public async Task<IActionResult> GetUserByToken([FromQuery] string token)
        {
            var user = await UserRepository.GetByTokenAsync(token);

            user.Password = null;

            return Ok(user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromQuery] Guid id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            await UserRepository.UpdateAsync(user);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User user)
        {
            var result = await UserService.Authenticate(user.Email, user.Password);

            if (result == null)
            {
                return Ok(null);
            }

            result.Password = null;

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("check-availability")]
        public async Task<IActionResult> CheckAvailability([FromBody] User user)
        {
            var result = await UserService.CheckAvailability(user.Email);

            return Ok(result);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout([FromQuery] string token)
        {
            await UserService.Logout(token);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("organization-data"), ProducesResponseType(StatusCodes.Status200OK)]
        public JsonResult GetOrganizationData()
        {
            var result = new
            {
                ConnectionSettings.OrganizationUrl
            };

            return new JsonResult(result);
        }

        // POST: api/Users
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            user.LogicalName = "user";
            user.Name = user.FullName;
            user.CreatedOn = DateTime.Now;
            var password = user.Password;
            user.Password = _passwordHasher.HashPassword(password);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user = await UserRepository.CreateAsync(user);

            user.Password = null;

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromQuery] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await UserRepository.DeleteAsync(id);

            return Ok();
        }
    }
}