namespace WebSchool.ViewModels.Assignment
{
    public class AssignmentQuestionInputModel
    {
        public string Question { get; set; }

        public bool HasMultipleAnswers { get; set; }

        public QuestionAnswerInputModel[] Answers { get; set; }
    }
}
