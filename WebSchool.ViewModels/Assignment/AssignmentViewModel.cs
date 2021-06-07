namespace WebSchool.ViewModels.Assignment
{
    public class AssignmentViewModel
    {
        public string Title { get; set; }

        public int AllPoints { get; set; }

        public QuestionViewModel[] Questions { get; set; }
    }
}
