using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtualPhysicsLab.Data.Models
{
    public class MeshSettings : BaseModel
    {
        public string HexColor { get; set; }
        public ICollection<MeshSize> Sizes { get; set; }
        [Required]
        public Mesh Mesh { get; set; }
        [Required]
        public Guid MeshId { get; set; }
    }
}
