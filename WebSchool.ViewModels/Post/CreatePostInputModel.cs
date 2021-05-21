using System.ComponentModel.DataAnnotations;

namespace WebSchool.ViewModels.Post
{
    public class CreatePostInputModel
    {
        [MinLength(5), MaxLength(1000)]
        public string Content { get; set; }

        [Required]
        public string GroupId { get; set; }
    }
}
