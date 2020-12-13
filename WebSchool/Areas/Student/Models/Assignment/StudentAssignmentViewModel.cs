using System;

namespace WebSchool.Models.Assignment
{
    public class StudentAssignmentViewModel
    {
        public string Id { get; set; }

        public string AssignmentName { get; set; }

        public string Signature { get; set; }

        public DateTime DueDate { get; set; }

        public int Points { get; set; }

        public byte Stage { get; set; }
    }
}
