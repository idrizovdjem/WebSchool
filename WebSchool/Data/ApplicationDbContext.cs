﻿using WebSchool.Data.Models;
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

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<UserClass> UserClasses { get; set; }

        public DbSet<UserSchool> UserSchools { get; set; }

        public DbSet<UserSubject> UserSubjects { get; set; }

        public DbSet<SchoolClass> SchoolClasses { get; set; }

        public DbSet<RegistrationLink> RegistrationLinks { get; set; }

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

            builder.Entity<UserSchool>(entity =>
            {
                entity.HasKey(us => new { us.UserId, us.SchoolId });

                entity
                    .HasOne(us => us.User)
                    .WithMany(u => u.Schools)
                    .OnDelete(DeleteBehavior.NoAction);

                entity
                    .HasOne(us => us.School)
                    .WithMany(s => s.Users)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<UserSubject>(entity =>
            {
                entity
                    .HasOne(us => us.User)
                    .WithMany(u => u.Subjects)
                    .OnDelete(DeleteBehavior.NoAction);

                entity
                    .HasOne(us => us.Subject)
                    .WithMany(s => s.Users)
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
