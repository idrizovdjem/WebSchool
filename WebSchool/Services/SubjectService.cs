using System;
using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using WebSchool.Models.Subject;
using System.Collections.Generic;
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

        public ICollection<SubjectViewModel> GetSubjects(string schoolId)
        {
            return this.context.Subjects
                .Where(x => x.SchoolId == schoolId)
                .Select(x => new SubjectViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    CreatedOn = x.CreatedOn
                })
                .ToList();
        }

        public async Task Remove(string id)
        {
            var subject = this.context.Subjects
                .FirstOrDefault(x => x.Id == id);

            if (subject == null)
            {
                return;
            }

            this.context.Subjects.Remove(subject);
            await this.context.SaveChangesAsync();
        }
    }
}
