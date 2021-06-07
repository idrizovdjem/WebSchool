namespace WebSchool.Common.Constants
{
    public static class PostConstants
    {
        public const byte MinimumContentLength = 5;

        public const ushort MaximumContentLength = 5000;

        public const byte MinimumTitleLength = 5;

        public const byte MaximumTitleLength = 150;

        public const byte MostPopularPostsCount = 10;

        public const byte MaximumTitlePreviewLength = 50;

        public const ushort MaximumContentPreviewLength = 500;

        public const string InvalidContentLengthMessage = "Post content must be between 5 and 5000 symbols";

        public const string ContentIsRequiredMessage = "Post content is required";

        public const string InvalidTitleLengthMessage = "Post title must be between 5 and 150 symbols";

        public const string TitleIsRequiredMessage = "Post title is required";
    }
}
