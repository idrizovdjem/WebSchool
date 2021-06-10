using WebSchool.ViewModels.Question;

namespace WebSchool.ViewModels.Assignment
{
    public class AssignmentResultPreviewViewModel
    {
        public string Title { get; set; }

        public QuestionPreviewViewModel[] Questions { get; set; }
    }
}
