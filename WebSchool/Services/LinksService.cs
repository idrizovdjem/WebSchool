using System;
using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebSchool.Services.Contracts;
using WebSchool.Models.RegistrationLink;

namespace WebSchool.Services
{
    public class LinksService : ILinksService
    {
        private readonly ApplicationDbContext context;

        public LinksService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task Delete(string id)
        {
            var link = this.context.RegistrationLinks
                .FirstOrDefault(x => x.Id == id);

            if (link == null)
            {
                return;
            }

            this.context.RegistrationLinks.Remove(link);
            await this.context.SaveChangesAsync();
        }

        public async Task<RegistrationLink> GenerateAdminLink(string email)
        {
            var link = new RegistrationLink()
            {
                RoleName = "Admin",
                From = "System",
                To = email
            };

            await this.context.RegistrationLinks.AddAsync(link);
            await this.context.SaveChangesAsync();

            return link;
        }

        public async Task<IEnumerable<RegistrationLink>> GenerateLinks(string roleName, string from, string schoolId, string[] toEmails)
        {
            var links = new List<RegistrationLink>();
            for (int i = 0; i < toEmails.Length; i++)
            {
                var link = new RegistrationLink()
                {
                    RoleName = roleName,
                    From = from,
                    To = toEmails[i],
                    SchoolId = schoolId,
                    CreatedOn = DateTime.UtcNow,
                    IsUsed = false
                };

                links.Add(link);
            }

            await this.context.RegistrationLinks.AddRangeAsync(links);
            await this.context.SaveChangesAsync();

            return links;
        }

        public ICollection<RegistrationLinkViewModel> GetGeneratedLinks(string adminId)
        {
            return this.context.RegistrationLinks
                .Where(x => x.From == adminId)
                .Select(x => new RegistrationLinkViewModel()
                {
                    Id = x.Id,
                    Email = x.To,
                    CreatedOn = x.CreatedOn,
                    Role = x.RoleName,
                    IsUsed = x.IsUsed == true ? "Yes" : "No"
                })
                .OrderByDescending(x => x.CreatedOn)
                .ToList();
        }

        public RegistrationLink GetLink(string registrationLinkId)
        {
            return this.context.RegistrationLinks
                .FirstOrDefault(x => x.Id == registrationLinkId);
        }

        public bool IsRoleValid(string role)
        {
            return this.context.Roles
                .Any(x => x.Name == role);
        }

        public async Task UseLink(string registrationLinkId)
        {
            var link = GetLink(registrationLinkId);
            if (link == null)
            {
                return;
            }

            link.IsUsed = true;
            await this.context.SaveChangesAsync();
        }
    }
}
