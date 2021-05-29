using System.ComponentModel.DataAnnotations;

namespace WebSchool.ViewModels.Users
{
    public class MemberInputModel
    {
        [Required]
        public string MemberId { get; set; }

        [Required]
        public string GroupId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
