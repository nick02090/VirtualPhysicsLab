﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualPhysicsLab.Data.Models
{
    [NotMapped]
    public abstract class BaseModel
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LogicalName { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is BaseModel)) return false;
            var temp = (BaseModel)obj;
            return temp.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
