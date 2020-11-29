using System.ComponentModel.DataAnnotations;

namespace WebSchool.Models.Comment
{
    public class CreateCommentInputModel
    {
        [Required]
        public string PostId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
