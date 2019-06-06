using System;
using VirtualPhysicsLab.Web.Models;

namespace VirtualPhysicsLab.Data.Models
{
    public class MeshSettings : BaseModel
    {
        public Vector3<float> Color { get; set; }
        public Vector3<float> Size { get; set; }
        public Vector3<float> Position { get; set; }
        public Vector3<float> Rotation { get; set; }
        public float Mass { get; set; }
        public float Friction { get; set; }
        public float Restitution { get; set; }
        public bool Axis { get; set; }
        public Mesh Mesh { get; set; }
        public Guid MeshId { get; set; }
    }
}
