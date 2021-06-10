using WebSchool.ViewModels.Answer;

namespace WebSchool.ViewModels.Question
{
    public class QuestionPreviewViewModel
    {
        public string Question { get; set; }

        public bool HasMultipleAnswers { get; set; }

        public byte Points { get; set; }

        public bool IsCorrect { get; set; }

        public AnswerViewModel[] Answers { get; set; }
    }
}
