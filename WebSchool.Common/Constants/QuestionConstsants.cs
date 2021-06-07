namespace WebSchool.Common.Constants
{
    public static class QuestionConstsants
    {
        public const int MinimumQuestionLength = 1;

        public const int MaximumQuestionLength = 5000;

        public const int MinimumAnswersCount = 1;

        public const int MaximumAnswersCount = 20;

        public const int MinimumPoints = 0;

        public const int MaximumPoints = 100;

        public const string QuestionIsRequiredMessage = "Question is required";

        public const string QuestionLengthMessage = "Question must be between 1 and 5000 symbols";

        public const string AnswersLengthMessage = "Answers must be between 1 and 20";

        public const string InvalidPointsMessage = "Points must be between 0 and 100";
    }
}
