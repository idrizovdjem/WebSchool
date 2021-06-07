namespace WebSchool.Common.Constants
{
    public static class AssignmentConstants
    {
        public const byte MinimumTitleLength = 3;

        public const byte MaximumTitleLength = 200;

        public const byte MinimumQuestionsCount = 1;

        public const byte MaximumQuestionsCount = 100;

        public const string TitleLengthMessage = "Title must be between 3 and 200 symbols";

        public const string TitleIsRequiredMessage = "Title is required";

        public const string InvalidQuestionsCountMessage = "Questions must be between 1 and 100";
    }
}
