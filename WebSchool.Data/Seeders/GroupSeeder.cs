using System;
using System.Linq;

using WebSchool.Data.Models;
using WebSchool.Common.Constants;

namespace WebSchool.Data.Seeders
{
    public class GroupSeeder : ISeeder
    {
        private readonly ApplicationDbContext dbContext;

        public GroupSeeder(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Seed()
        {
            if(dbContext.Groups.Any(g => g.Name == GroupConstants.GlobalGroupName) == false)
            {
                var globalGroup = new Group()
                {
                    Name = GroupConstants.GlobalGroupName,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false
                };

                dbContext.Groups.AddAsync(globalGroup).GetAwaiter().GetResult();
                dbContext.SaveChangesAsync().GetAwaiter().GetResult();
            }
        }
    }
}
