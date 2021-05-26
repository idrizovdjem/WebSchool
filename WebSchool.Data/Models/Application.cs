using System.ComponentModel.DataAnnotations;

namespace WebSchool.Data.Models
{
    public class Application
    {
        [Required]
        public string GroupId { get; set; }

        public Group Group { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public bool IsConfirmed { get; set; }
    }
}
