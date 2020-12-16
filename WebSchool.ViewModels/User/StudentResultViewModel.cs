using System;
using System.ComponentModel.DataAnnotations;

namespace WebSchool.ViewModels.User
{
    public class StudentResultViewModel
    {
        public string StudentId { get; set; }

        public string AssignmentId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DueDate { get; set; }

        public int Points { get; set; }

        public int MaxPoints { get; set; }

        public byte Stage { get; set; }
    }
}
