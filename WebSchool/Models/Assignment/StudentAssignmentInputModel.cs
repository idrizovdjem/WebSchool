﻿using System;

namespace WebSchool.Models.Assignment
{
    public class StudentAssignmentInputModel
    {
        public string Id { get; set; }

        public string AssignmentName { get; set; }

        public string Signature { get; set; }

        public DateTime DueDate { get; set; }

        public byte Stage { get; set; }
    }
}
