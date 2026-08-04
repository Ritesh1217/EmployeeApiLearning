using EmployeeApiLearning.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApiLearning.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<AppUser> Users { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.EmployeeCode)
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Employee)
                .WithOne(e =>  e.User)
                .HasPrincipalKey<Employee>(e => e.EmployeeCode)
                .HasForeignKey<Employee>(u => u.EmployeeCode);
        }
    }
}