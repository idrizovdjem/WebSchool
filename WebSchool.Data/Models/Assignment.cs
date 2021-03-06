﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using WebSchool.Common.Constants;

namespace WebSchool.Data.Models
{
    public class Assignment
    {
        public Assignment()
        {
            Id = Guid.NewGuid().ToString();
            Groups = new HashSet<GivenAssignment>();
            Results = new HashSet<AssignmentResult>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(AssignmentConstants.MaximumTitleLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<GivenAssignment> Groups { get; set; }

        public virtual ICollection<AssignmentResult> Results { get; set; }
    }
}
