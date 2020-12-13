using System;
using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using WebSchool.Models.User;
using System.Threading.Tasks;
using WebSchool.Models.Classes;
using System.Collections.Generic;
using WebSchool.Services.Contracts;
using WebSchool.Areas.Admin.Models.Classes;

namespace WebSchool.Services
{
    public class ClassesService : IClassesService
    {
        private readonly ApplicationDbContext context;
        private readonly IUsersService usersService;
        private readonly IRolesService rolesService;

        public ClassesService(ApplicationDbContext context, IUsersService usersService, IRolesService rolesService)
        {
            this.context = context;
            this.usersService = usersService;
            this.rolesService = rolesService;
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

        public async Task AssignUserToClass(string userId, string signature, string schoolId)
        {
            var schoolClass = this.context.SchoolClasses
                .FirstOrDefault(x => x.Signature == signature && x.SchoolId == schoolId);

            if (schoolClass == null)
            {
                return;
            }

            var userClass = new UserClass()
            {
                SchoolClassId = schoolClass.Id,
                UserId = userId
            };

            await this.context.UserClasses.AddAsync(userClass);
            await this.context.SaveChangesAsync();
        }

        public bool ClassExists(string signature, string schoolId)
        {
            return this.context.SchoolClasses.Any(x => x.SchoolId == schoolId && x.Signature == signature);
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

        public ICollection<TeacherClassViewModel> GetClassesWithoutTeacher(string teacherId, string schoolId)
        {
            var teacherClasses = this.context.UserClasses
                .Where(x => x.UserId == teacherId)
                .Select(x => x.SchoolClassId)
                .ToList();

            var schoolClasses = this.context.SchoolClasses
                .Where(x => x.SchoolId == schoolId)
                .Select(x => new TeacherClassViewModel()
                {
                    Id = x.Id,
                    Signature = x.Signature
                })
                .ToList();

            return schoolClasses
                .Where(x => !teacherClasses.Contains(x.Id))
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
                    Id = x.User.Id,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    Email = x.User.Email
                })
                .ToList()
                .Where(x => this.rolesService.GetUserRole(x.Id) == "Student")
                .ToList();

            schoolClass.Students = students;

            return schoolClass;
        }

        public ICollection<string> GetStudentsFromClass(string signature, string schoolId)
        {
            var schoolClass = this.context.SchoolClasses
                .FirstOrDefault(x => x.SchoolId == schoolId && x.Signature == signature);

            if (schoolClass == null)
            {
                return new List<string>();
            }

            var userIds = this.context.UserClasses
                .Where(x => x.SchoolClassId == schoolClass.Id)
                .Select(x => x.UserId)
                .ToList();

            var students = new List<string>();
            foreach (var userId in userIds)
            {
                if (this.rolesService.GetUserRole(userId) == "Student")
                {
                    students.Add(userId);
                }
            }

            return students;
        }

        public ICollection<TeacherClassViewModel> GetTeacherAssignedClasses(string teacherId)
        {
            var userClasses = this.context.UserClasses
                .Where(x => x.UserId == teacherId)
                .Select(x => x.SchoolClassId)
                .ToList();
            return this.context.SchoolClasses
                .Where(x => userClasses.Contains(x.Id))
                .Select(x => new TeacherClassViewModel()
                {
                    Id = x.Id,
                    Signature = x.Signature,
                })
                .ToList();
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

        public async Task RemoveClassFromUser(string classId, string userId)
        {
            var userClass = this.context.UserClasses
                .FirstOrDefault(x => x.UserId == userId && x.SchoolClassId == classId);

            if (userClass == null)
            {
                return;
            }

            this.context.UserClasses.Remove(userClass);
            await this.context.SaveChangesAsync();
        }
    }
}
