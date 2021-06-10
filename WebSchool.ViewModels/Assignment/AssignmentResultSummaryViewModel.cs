namespace WebSchool.ViewModels.Assignment
{
    public class AssignmentResultSummaryViewModel
    {
        public string Title { get; set; }

        public double AveragePoints { get; set; }

        public AssignmentResultViewModel[] Results { get; set; }
    }
}
