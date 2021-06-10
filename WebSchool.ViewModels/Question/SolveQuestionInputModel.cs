using WebSchool.ViewModels.Answer;

namespace WebSchool.ViewModels.Question
{
    public class SolveQuestionInputModel
    {
        public bool IsCorrect { get; set; }

        public SolveAnswerInputModel[] Answers { get; set; }
    }
}
