namespace WebSchool.ViewModels.Assignment
{
    public class AssignmentViewModel
    {
        public string Title { get; set; }

        public int Points { get; set; }

        public QuestionViewModel[] Questions { get; set; }
    }
}
