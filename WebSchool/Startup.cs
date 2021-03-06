using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WebSchool.Data;
using WebSchool.Data.Models;
using WebSchool.Data.Seeders;
using WebSchool.Services.Posts;
using WebSchool.Services.Groups;
using WebSchool.Services.Common;
using WebSchool.Services.Assignments;
using WebSchool.Services.Administration;

namespace WebSchool
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddTransient<IPostsService, PostsService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IGroupsService, GroupsService>();
            services.AddTransient<IBrowseService, BrowseService>();
            services.AddTransient<IMembersService, MembersService>();
            services.AddTransient<IAnswersService, AnswersService>();
            services.AddTransient<ICommentsService, CommentsService>();
            services.AddTransient<IUtilitiesService, UtilitiesService>();
            services.AddTransient<IQuestionsService, QuestionsService>();
            services.AddTransient<IAssignmentsService, AssignmentsService>();
            services.AddTransient<IApplicationsService, ApplicationsService>();
            services.AddTransient<INotificationsService, NotificationsService>();
            services.AddTransient<IAdministrationService, AdministrationService>();

            services.AddAntiforgery();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<ApplicationRole> roleManager, ApplicationDbContext context)
        {
            new RoleSeeder(roleManager).Seed();
            new GroupSeeder(context).Seed();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "MyArea",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "apiControllers",
                    pattern: "{apicontroller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
