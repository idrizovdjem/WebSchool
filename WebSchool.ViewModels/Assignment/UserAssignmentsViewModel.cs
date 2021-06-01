namespace WebSchool.ViewModels.Assignment
{
    public class UserAssignmentsViewModel
    {
        public AssignmentPreviewViewModel[] CreatedAssignments { get; set; }

        public AssignmentPreviewViewModel[] NotSolvedAssignments { get; set; }

        public AssignmentPreviewViewModel[] SolvedAssignments { get; set; }
    }
}
