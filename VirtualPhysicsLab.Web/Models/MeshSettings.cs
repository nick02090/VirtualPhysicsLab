using System;
using VirtualPhysicsLab.Web.Models;

namespace VirtualPhysicsLab.Data.Models
{
    public class MeshSettings : BaseModel
    {
        public Color Color { get; set; }
        public Size Size { get; set; }
        public Position Position { get; set; }
        public Rotation Rotation { get; set; }
        public Velocity Velocity { get; set; }
        public float Mass { get; set; }
        public float Friction { get; set; }
        public float Restitution { get; set; }
        public bool Axis { get; set; }
        public Mesh Mesh { get; set; }
        public Guid MeshId { get; set; }
    }
}
