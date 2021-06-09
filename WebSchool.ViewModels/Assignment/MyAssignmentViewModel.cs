using System;

using WebSchool.Common.Enumerations;

namespace WebSchool.ViewModels.Assignment
{
    public class MyAssignmentViewModel
    {
        public string Title { get; set; }

        public string GroupName { get; set; }

        public string GroupAssignmentId { get; set; }

        public GivenAssignmentStatus Status { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsSolved { get; set; }
    }
}
