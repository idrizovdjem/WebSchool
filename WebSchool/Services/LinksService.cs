using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebSchool.Services.Contracts;

namespace WebSchool.Services
{
    public class LinksService : ILinksService
    {
        private readonly ApplicationDbContext context;

        public LinksService(ApplicationDbContext context)
        {
            this.context = context;
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

        public async Task<IEnumerable<RegistrationLink>> GenerateLinks(string roleName, string from, string[] toEmails)
        {
            var links = new List<RegistrationLink>();
            for(int i = 0; i < toEmails.Length; i++)
            {
                var link = new RegistrationLink()
                {
                    RoleName = roleName,
                    From = from,
                    To = toEmails[i]
                };

                links.Add(link);
            }

            await this.context.RegistrationLinks.AddRangeAsync(links);
            await this.context.SaveChangesAsync();

            return links;
        }

        public RegistrationLink GetLink(string registrationLinkId)
        {
            return this.context.RegistrationLinks
                .FirstOrDefault(x => x.Id == registrationLinkId);
        }

        public async Task UseLink(string registrationLinkId)
        {
            var link = GetLink(registrationLinkId);
            if(link == null)
            {
                return;
            }

            link.IsUsed = true;
            await this.context.SaveChangesAsync();
        }
    }
}
