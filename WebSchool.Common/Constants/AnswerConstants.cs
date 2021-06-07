namespace WebSchool.Common.Constants
{
    public static class AnswerConstants
    {
        public const byte MinimumContentLength = 1;

        public const ushort MaximumContentLength = 500;

        public const string ContentIsRequiredMessage = "Content is required";

        public const string ContentLengthMessage = "Content must be between 1 and 500 symbols";

        public const string MissingCorrectAnswerMessage = "At least 1 correct answer must be present";

        public const string TooMuchCorrectAnswersMessage = "There are too much correct answers";
    }
}
