using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtualPhysicsLab.Data.Models
{
    public class Experiment : BaseModel
    {
        public ICollection<Mesh> Meshes { get; set; }
        [Required]
        public User CreatedBy { get; set; }
    }
}
