using System.ComponentModel.DataAnnotations;

namespace WebSchool.Data.Models
{
    public class UserSchool
    {
        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string SchoolId { get; set; }

        public virtual School School { get; set; }
    }
}