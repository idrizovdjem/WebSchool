using System;

namespace WebSchool.ViewModels.Assignment
{
    public class AssignmentInformationViewModel
    {
        public string Id { get; set; }

        public string AssignmentName { get; set; }

        public string Signature { get; set; }

        public DateTime DueDate { get; set; }
    }
}
