using WebSchool.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebSchool.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<School> Schools { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<UserClass> UserClasses { get; set; }

        public DbSet<SchoolClass> SchoolClasses { get; set; }

        public DbSet<RegistrationLink> RegistrationLinks { get; set; }

        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<AssignmentResult> AssignmentResults { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("Server=.;Database=WebSchool;Integrated Security=true;");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Post>(entity =>
            {
                entity
                    .HasOne(p => p.School)
                    .WithMany(s => s.Posts)
                    .OnDelete(DeleteBehavior.NoAction);

                entity
                    .HasOne(p => p.Creator)
                    .WithMany(c => c.Posts)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Comment>(entity =>
            {
                entity
                    .HasOne(c => c.Post)
                    .WithMany(p => p.Comments)
                    .OnDelete(DeleteBehavior.NoAction);

                entity
                    .HasOne(c => c.Creator)
                    .WithMany(c => c.Comments)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<UserClass>(entity =>
            {
                entity
                    .HasOne(us => us.SchoolClass)
                    .WithMany(sc => sc.Users)
                    .OnDelete(DeleteBehavior.NoAction);

                entity
                    .HasOne(us => us.User)
                    .WithMany(u => u.Classes)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<SchoolClass>(entity =>
            {
                entity
                    .HasOne(sc => sc.School)
                    .WithMany(s => s.Classes)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            base.OnModelCreating(builder);
        }
    }
}
