using System;
using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using WebSchool.Models.Classes;
using System.Collections.Generic;
using WebSchool.Services.Contracts;

namespace WebSchool.Services
{
    public class ClassesService : IClassesService
    {
        private readonly ApplicationDbContext context;

        public ClassesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task CreateClass(string signature, string schoolId)
        {
            var schoolClass = new SchoolClass()
            {
                SchoolId = schoolId,
                Signature = signature,
                CreatedOn = DateTime.UtcNow
            };

            await this.context.SchoolClasses.AddAsync(schoolClass);
            await this.context.SaveChangesAsync();
        }

        public ICollection<ClassViewModel> GetClasses(string schoolId)
        {
            return this.context.SchoolClasses
                .Where(x => x.SchoolId == schoolId)
                .Select(x => new ClassViewModel()
                {
                    Signature = x.Signature,
                    SchoolId = x.SchoolId,
                    CreatedOn = x.CreatedOn
                })
                .ToList();
        }

        public bool IsClassSignatureAvailable(string signature, string schoolId)
        {
            return !this.context.SchoolClasses.Any(x => x.Signature == signature && x.SchoolId == schoolId);
        }
    }
}
