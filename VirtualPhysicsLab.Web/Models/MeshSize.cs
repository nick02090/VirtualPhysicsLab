using System.ComponentModel.DataAnnotations;

namespace VirtualPhysicsLab.Data.Models
{
    public class MeshSize : BaseModel
    {
        [Required]
        public MeshSettings Settings { get; set; }
        [Required]
        public decimal Value { get; set; }
    }
}
