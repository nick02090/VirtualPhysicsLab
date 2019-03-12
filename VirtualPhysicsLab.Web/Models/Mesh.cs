using System.ComponentModel.DataAnnotations;
using VirtualPhysicsLab.Data.Enums;

namespace VirtualPhysicsLab.Data.Models
{
    public class Mesh : BaseModel
    {
        [Required]
        public MeshType Type { get; set; }
        [Required]
        public Experiment Experiment { get; set; }
        public MeshSettings Settings { get; set; }
    }
}
