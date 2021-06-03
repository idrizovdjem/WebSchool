using System.ComponentModel.DataAnnotations;

namespace WebSchool.ViewModels.Post
{
    public class EditPostInputModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [MinLength(5), MaxLength(150)]
        public string Title { get; set; }

        [Required]
        [MinLength(5), MaxLength(5000)]
        public string Content { get; set; }
    }
}
