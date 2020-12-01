using System.ComponentModel.DataAnnotations;

namespace WebSchool.Data.Models
{
    public class UserClass
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string SchoolClassId { get; set; }

        public virtual SchoolClass SchoolClass { get; set; }
    }
}