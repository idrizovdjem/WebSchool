using System;
using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using WebSchool.Models.User;
using System.Threading.Tasks;
using WebSchool.Models.Classes;
using System.Collections.Generic;
using WebSchool.Services.Contracts;

namespace WebSchool.Services
{
    public class ClassesService : IClassesService
    {
        private readonly ApplicationDbContext context;
        private readonly IUsersService usersService;

        public ClassesService(ApplicationDbContext context, IUsersService usersService)
        {
            this.context = context;
            this.usersService = usersService;
        }

        public async Task AddStudentsToClass(string signature, List<string> emails, string schoolId)
        {
            var userClasses = new List<UserClass>();
            foreach (var email in emails)
            {
                var userClass = new UserClass()
                {
                    UserId = this.usersService.GetUserByEmail(email).Id,
                    SchoolClassId = this.GetClassInformation(signature, schoolId).Id,
                };

                userClasses.Add(userClass);
            }

            await this.context.UserClasses.AddRangeAsync(userClasses);
            await this.context.SaveChangesAsync();
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

        public SchoolClassViewModel GetClassInformation(string signature, string schoolId)
        {
            var schoolClass = this.context.SchoolClasses
                .Where(x => x.Signature == signature && x.SchoolId == schoolId)
                .Select(x => new SchoolClassViewModel()
                {
                    Id = x.Id,
                    Signature = x.Signature,
                })
                .FirstOrDefault();

            var students = this.context.UserClasses
                .Where(x => x.SchoolClassId == schoolClass.Id)
                .Select(x => new StudentClassViewModel()
                {
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    Email = x.User.Email
                })
                .ToList();

            schoolClass.Students = students;

            return schoolClass;
        }

        public bool IsClassSignatureAvailable(string signature, string schoolId)
        {
            return !this.context.SchoolClasses.Any(x => x.Signature == signature && x.SchoolId == schoolId);
        }

        public async Task Remove(string signature, string email, string schoolId)
        {
            var schoolClass = this.context.SchoolClasses
                .FirstOrDefault(x => x.Signature == signature && x.SchoolId == schoolId);

            var userClass = this.context.UserClasses
                .FirstOrDefault(x => x.SchoolClassId == schoolClass.Id && x.User.Email == email);

            this.context.UserClasses.Remove(userClass);
            await this.context.SaveChangesAsync();
        }
    }
}
