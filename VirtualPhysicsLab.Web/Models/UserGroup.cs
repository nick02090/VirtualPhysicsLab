using System;
using System.ComponentModel.DataAnnotations;

namespace VirtualPhysicsLab.Data.Models
{
    public class UserGroup : BaseModel
    {
        [Required]
        public User User { get; set; }
        [Required]
        public Group Group { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid GroupId { get; set; }
    }
}
