using System.ComponentModel.DataAnnotations;

namespace WebSchool.Models.User
{
    public class AssignClassToTeacherInputModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Signature { get; set; }
    }
}
