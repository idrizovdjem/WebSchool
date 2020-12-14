using System.ComponentModel.DataAnnotations;

namespace WebSchool.ViewModels.Post
{
    public class CreatePostInputModel
    {
        [Required]
        public string Content { get; set; }
    }
}
