using System;

using Microsoft.AspNetCore.Identity;

namespace WebSchool.Data.BaseModels
{
    public class BaseUserModel : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
