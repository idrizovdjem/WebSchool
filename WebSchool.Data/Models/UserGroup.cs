using System.ComponentModel.DataAnnotations;

namespace WebSchool.Data.Models
{
    public class UserGroup
    {
        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string GroupId { get; set; }

        public virtual Group Group { get; set; }

        [Required]
        public string RoleId { get; set; }

        public virtual ApplicationRole Role { get; set; }
    }
}
