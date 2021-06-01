using System.ComponentModel.DataAnnotations;

namespace WebSchool.ViewModels.Post
{
    public class CreatePostInputModel
    {
        [Required]
        [MinLength(5), MaxLength(5000)]
        public string Content { get; set; }

        [Required]
        [MinLength(5), MaxLength(150)]
        public string Title { get; set; }

        [Required]
        public string GroupId { get; set; }
    }
}
