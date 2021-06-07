namespace WebSchool.Common.Constants
{
    public static class QuestionConstsants
    {
        public const byte MinimumQuestionLength = 1;

        public const ushort MaximumQuestionLength = 5000;

        public const byte MinimumAnswersCount = 1;

        public const byte MaximumAnswersCount = 20;

        public const byte MinimumPoints = 0;

        public const byte MaximumPoints = 100;

        public const string QuestionIsRequiredMessage = "Question is required";

        public const string QuestionLengthMessage = "Question must be between 1 and 5000 symbols";

        public const string AnswersLengthMessage = "Answers must be between 1 and 20";

        public const string InvalidPointsMessage = "Points must be between 0 and 100";
    }
}
