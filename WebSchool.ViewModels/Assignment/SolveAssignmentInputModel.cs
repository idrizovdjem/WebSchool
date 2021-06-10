using WebSchool.ViewModels.Question;

namespace WebSchool.ViewModels.Assignment
{
    public class SolveAssignmentInputModel
    {
        public string groupAssignmentId { get; set; }

        public SolveQuestionInputModel[] Questions { get; set; }
    }
}
