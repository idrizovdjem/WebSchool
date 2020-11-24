using System;
using System.ComponentModel.DataAnnotations;

namespace WebSchool.Data.Models
{
    public class RegistrationLink
    {
        public RegistrationLink()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string RoleName { get; set; }

        public bool IsUsed { get; set; }
    }
}
