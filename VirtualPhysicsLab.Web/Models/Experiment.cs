using System.Collections.Generic;
using VirtualPhysicsLab.Web.Enums;
using VirtualPhysicsLab.Web.Models;

namespace VirtualPhysicsLab.Data.Models
{
    public class Experiment : BaseModel
    {
        public ExperimentSettings Settings { get; set; }
        public ICollection<Mesh> Meshes { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Picture Picture { get; set; }
        public User CreatedBy { get; set; }
    }
}
