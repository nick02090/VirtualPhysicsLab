﻿using System;
using VirtualPhysicsLab.Data.Models;

namespace VirtualPhysicsLab.Web.Models
{
    public class Position : Vector3<float>
    {
        public MeshSettings MeshSettings { get; set; }
        public Guid MeshSettingsId { get; set; }
    }
}
