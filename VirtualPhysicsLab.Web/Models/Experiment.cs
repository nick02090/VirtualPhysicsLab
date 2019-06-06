using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtualPhysicsLab.Web.Models;

namespace VirtualPhysicsLab.Data.Models
{
    public class Experiment : BaseModel
    {
        public ExperimentSettings Settings { get; set; }
        public ICollection<Mesh> Meshes { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public User CreatedBy { get; set; }
    }
}
