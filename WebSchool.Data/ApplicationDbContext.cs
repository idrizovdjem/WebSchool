using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using WebSchool.Data.Models;

namespace WebSchool.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Group> Groups { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<UserAssignment> UserAssignments { get; set; }

        public DbSet<AssignmentResult> AssignmentResults { get; set; }

        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Post>(entity =>
            {
                entity
                    .HasOne(p => p.Group)
                    .WithMany(g => g.Posts)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(p => p.Creator)
                    .WithMany(c => c.Posts)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Comment>(entity =>
            {
                entity
                    .HasOne(c => c.Post)
                    .WithMany(p => p.Comments)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(c => c.Creator)
                    .WithMany(c => c.Comments)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<UserGroup>(entity =>
            {
                entity.HasKey(x => new { x.UserId, x.GroupId });

                entity
                    .HasOne(ug => ug.User)
                    .WithMany(u => u.Groups)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(ug => ug.Group)
                    .WithMany(g => g.Users)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Application>(entity =>
            {
                entity.HasKey(ap => new { ap.GroupId, ap.UserId });
            });

            builder.Entity<UserAssignment>(entity =>
            {
                entity.HasKey(ua => new { ua.StudentId, ua.AssignmentId });

                entity
                    .HasOne(ua => ua.Student)
                    .WithMany(s => s.Assignments)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(ua => ua.Assignment)
                    .WithMany(a => a.Students)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<AssignmentResult>(entity =>
            {
                entity
                    .HasOne(ar => ar.Assignment)
                    .WithMany(a => a.Results)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(ar => ar.Student)
                    .WithMany(s => s.Results)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(builder);
        }
    }
}
