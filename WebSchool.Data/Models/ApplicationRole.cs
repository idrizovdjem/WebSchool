using System;

using Microsoft.AspNetCore.Identity;

namespace WebSchool.Data.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
            : this(null)
        {
        }

        public ApplicationRole(string name)
            : base(name)
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
