using System;

namespace VirtualPhysicsLab.Web.Models
{
    public class Gravity : Vector3<float>
    {
        public int Axis { get; set; }
        public ExperimentSettings ExperimentSettings { get; set; }
        public Guid ExperimentSettingsId { get; set; }
    }
}
