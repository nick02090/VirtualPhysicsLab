using System;

namespace VirtualPhysicsLab.Web.Models
{
    public class SceneSize : Vector3<float>
    {
        public ExperimentSettings ExperimentSettings { get; set; }
        public Guid ExperimentSettingsId { get; set; }
    }
}
