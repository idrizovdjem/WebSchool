using System.ComponentModel.DataAnnotations;

namespace WebSchool.ViewModels.Comment
{
    public class EditCommentInputModel
    {
        public string Id { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
