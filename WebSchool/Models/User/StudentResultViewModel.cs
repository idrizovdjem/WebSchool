using System;

namespace WebSchool.Models.User
{
    public class StudentResultViewModel
    {
        public string StudentId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DueDate { get; set; }

        public int Points { get; set; }

        public byte Stage { get; set; }
    }
}
