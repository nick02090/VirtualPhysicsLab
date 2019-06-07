using System;
using VirtualPhysicsLab.Data.Models;

namespace VirtualPhysicsLab.Web.Models
{
    public class Color : BaseModel
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public MeshSettings MeshSettings { get; set; }
        public Guid MeshSettingsId { get; set; }
    }
}
