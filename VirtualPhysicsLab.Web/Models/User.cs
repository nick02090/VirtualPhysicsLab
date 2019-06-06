using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VirtualPhysicsLab.Web.Enums;

namespace VirtualPhysicsLab.Data.Models
{
    public class User : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public Occupation Occupation { get; set; }
        public string Token { get; set; }
        public ICollection<UserGroup> Groups { get; set; }
        public ICollection<Experiment> Experiments { get; set; }
    }
}
