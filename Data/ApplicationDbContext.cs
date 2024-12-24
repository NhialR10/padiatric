using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Padiatric.Models;

namespace Padiatric.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Assistant> Assistants { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Emergency> Emergencies { get; set; }
        public DbSet<Announcement> Announcements { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TPT Inheritance Mapping
            modelBuilder.Entity<Assistant>()
                .ToTable("Assistants")
                .HasBaseType<AppUser>();

            modelBuilder.Entity<Professor>()
                .ToTable("Professors")
                .HasBaseType<AppUser>();

            modelBuilder.Entity<Admin>()
                .ToTable("Admins")
                .HasBaseType<AppUser>();

            // Department-Professor Relationship
            modelBuilder.Entity<Professor>()
                .HasOne(p => p.Department)
                .WithMany(d => d.Professors)
                .HasForeignKey(p => p.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade); // Enable cascade delete for professors

            // Shift Entity Relationships
            modelBuilder.Entity<Shift>()
                .HasOne(s => s.Assistant)
                .WithMany(a => a.Shifts)
                .HasForeignKey(s => s.AssistantId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Shift>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Shifts)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade); 

            // Appointment Entity Relationships
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Assistant)
                .WithMany(a => a.Appointments)
                .HasForeignKey(a => a.AssistantId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Professor)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.ProfessorId)
                .OnDelete(DeleteBehavior.Cascade); 
            // Emergency Entity Relationships
            modelBuilder.Entity<Emergency>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Emergencies)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
