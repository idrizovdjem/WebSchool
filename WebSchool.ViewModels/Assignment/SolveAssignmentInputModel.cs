using WebSchool.ViewModels.Question;

namespace WebSchool.ViewModels.Assignment
{
    public class SolveAssignmentInputModel
    {
        public string GroupAssignmentId { get; set; }

        public SolveQuestionInputModel[] Questions { get; set; }
    }
}
