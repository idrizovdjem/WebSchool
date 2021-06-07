namespace WebSchool.ViewModels.Assignment
{
    public class QuestionViewModel
    {
        public string Question { get; set; }

        public bool HasMultipleAnswers { get; set; }

        public byte Points { get; set; }

        public AnswerViewModel[] Answers { get; set; }
    }
}
