using WebSchool.Data;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebSchool.Services.Contracts;
using System.Linq;

namespace WebSchool.Services
{
    public class LinksService : ILinksService
    {
        private readonly ApplicationDbContext context;

        public LinksService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task GenerateLinks(string roleName, int count)
        {
            var links = new List<RegistrationLink>();
            for(int i = 0; i < count; i++)
            {
                var link = new RegistrationLink()
                {
                    RoleName = roleName
                };

                links.Add(link);
            }

            await this.context.AddRangeAsync(links);
            await this.context.SaveChangesAsync();
        }

        public RegistrationLink GetLink(string registrationLinkId)
        {
            return this.context.RegistrationLinks
                .FirstOrDefault(x => x.Id == registrationLinkId);
        }
    }
}
