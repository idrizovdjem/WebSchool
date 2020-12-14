using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebSchool.Data.Models
{
    public class SchoolClass
    {
        public SchoolClass()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Users = new HashSet<UserClass>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Signature { get; set; }

        [Required]
        public string SchoolId { get; set; }

        public virtual School School { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<UserClass> Users { get; set; }
    }
}