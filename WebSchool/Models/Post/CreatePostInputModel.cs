using System.ComponentModel.DataAnnotations;

namespace WebSchool.Models.Post
{
    public class CreatePostInputModel
    {
        [Required]
        public string Content { get; set; }
    }
}
