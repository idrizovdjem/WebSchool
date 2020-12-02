using System;
using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using WebSchool.Services.Contracts;

namespace WebSchool.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ApplicationDbContext context;

        public SubjectService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task CreateSubject(string title, string schoolId)
        {
            var subject = new Subject()
            {
                Title = title,
                SchoolId = schoolId,
                CreatedOn = DateTime.UtcNow
            };

            await this.context.Subjects.AddAsync(subject);
            await this.context.SaveChangesAsync();
        }

        public bool DoesSubjectExists(string subject, string schoolId)
        {
            return this.context.Subjects.Any(x => x.Title == subject && x.SchoolId == schoolId);
        }
    }
}
