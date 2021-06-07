using System.ComponentModel.DataAnnotations;

using WebSchool.Common.Constants;

namespace WebSchool.ViewModels.Post
{
    public class CreatePostInputModel
    {
        [Required(ErrorMessage = PostConstants.ContentIsRequiredMessage)]
        [MinLength(PostConstants.MinimumContentLength, ErrorMessage = PostConstants.InvalidContentLengthMessage)]
        [MaxLength(PostConstants.MaximumContentLength, ErrorMessage = PostConstants.InvalidContentLengthMessage)]
        public string Content { get; set; }

        [Required(ErrorMessage = PostConstants.TitleIsRequiredMessage)]
        [MinLength(PostConstants.MinimumTitleLength, ErrorMessage = PostConstants.InvalidTitleLengthMessage)]
        [MaxLength(PostConstants.MaximumTitleLength, ErrorMessage = PostConstants.InvalidTitleLengthMessage)]
        public string Title { get; set; }

        [Required]
        public string GroupId { get; set; }
    }
}
