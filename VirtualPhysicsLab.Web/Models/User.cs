using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VirtualPhysicsLab.Web.Enums;

namespace VirtualPhysicsLab.Data.Models
{
    public class User : BaseModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        [Required]
        public string NickName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public Occupation Occupation { get; set; }
        public string Token { get; set; }
        public ICollection<UserGroup> Groups { get; set; }
        public ICollection<Experiment> Experiments { get; set; }
    }
}
