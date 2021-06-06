namespace WebSchool.Common.Constants
{
    public class AnswerConstants
    {
        public const int MinimumContentLength = 1;

        public const int MaximumContentLength = 500;

        public const string ContentIsRequiredMessage = "Content is required";

        public const string ContentLengthMessage = "Content must be between 1 and 500 symbols";

        public const string MissingCorrectAnswerMessage = "At least 1 correct answer must be present";

        public const string TooMuchCorrectAnswersMessage = "There are too much correct answers";
    }
}
