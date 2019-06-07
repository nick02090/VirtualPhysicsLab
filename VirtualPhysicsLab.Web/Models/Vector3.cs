using System;
using VirtualPhysicsLab.Data.Models;

namespace VirtualPhysicsLab.Web.Models
{
    public class Vector3<T> : BaseModel
    {
        public T X { get; set; }
        public T Y { get; set; }
        public T Z { get; set; }
    }
}
