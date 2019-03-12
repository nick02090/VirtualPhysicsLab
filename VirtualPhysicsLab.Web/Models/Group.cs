using System.Collections.Generic;

namespace VirtualPhysicsLab.Data.Models
{
    public class Group : BaseModel
    {
        public ICollection<UserGroup> Users { get; set; }
    }
}
