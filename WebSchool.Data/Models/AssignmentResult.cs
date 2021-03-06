﻿using System;
using System.ComponentModel.DataAnnotations;

namespace WebSchool.Data.Models
{
    public class AssignmentResult
    {
        public AssignmentResult()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string groupAssignmentId { get; set; }

        public GivenAssignment GroupAssignment { get; set; }

        [Required]
        public string StudentId { get; set; }

        public virtual ApplicationUser Student { get; set; }

        public int Points { get; set; }

        public bool IsSolved { get; set; }

        public string Content { get; set; }
    }
}
