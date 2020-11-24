using System;
using System.ComponentModel.DataAnnotations;

namespace WebSchool.Data.Models
{
    public class UserSubject
    {
        public UserSubject()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string SubjectId { get; set; }

        public virtual Subject Subject { get; set; }
    }
}