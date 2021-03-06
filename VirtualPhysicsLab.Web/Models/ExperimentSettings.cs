﻿using System;
using VirtualPhysicsLab.Data.Models;

namespace VirtualPhysicsLab.Web.Models
{
    public class ExperimentSettings : BaseModel
    {
        public float Friction { get; set; }
        public float Restitution { get; set; }
        public bool Walls { get; set; }
        public bool Axis { get; set; }
        public Gravity Gravity { get; set; }
        public SceneSize Size { get; set; }
        public Experiment Experiment { get; set; }
        public Guid ExperimentId { get; set; }
    }
}
