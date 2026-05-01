using Microsoft.EntityFrameworkCore;
using EMS.API.Models;

namespace EMS.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<AppUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique Constraints
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Column Configurations
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(15);
                entity.Property(e => e.Department).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Designation).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).IsRequired().HasMaxLength(10);
            });

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Role).IsRequired().HasMaxLength(20);
            });

            // Seed Users
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = 20,
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow
                },
                new AppUser
                {
                    Id = 21,
                    Username = "viewer",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("viewer123"),
                    Role = "Viewer",
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Seed Employees (Sample 10 records)
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    FirstName = "Amit",
                    LastName = "Sharma",
                    Email = "amiat.sharma31@example.com",
                    Phone = "9876543210",
                    Department = "Engineering",
                    Designation = "Software Engineer",
                    Salary = 60000,
                    JoinDate = DateTime.UtcNow.AddDays(-120),
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow.AddDays(-120),
                    UpdatedAt = DateTime.UtcNow.AddDays(-120)
                },
new Employee
{
    Id = 2,
    FirstName = "Priya",
    LastName = "Mehta",
    Email = "pribya.mehta32@example.com",
    Phone = "9123456780",
    Department = "HR",
    Designation = "HR Manager",
    Salary = 55000,
    JoinDate = DateTime.UtcNow.AddDays(-200),
    Status = "Active",
    CreatedAt = DateTime.UtcNow.AddDays(-200),
    UpdatedAt = DateTime.UtcNow.AddDays(-200)
},
new Employee
{
    Id = 3,
    FirstName = "Rahul",
    LastName = "Verma",
    Email = "rcahul.verma33@example.com",
    Phone = "9988776655",
    Department = "Finance",
    Designation = "Accountant",
    Salary = 50000,
    JoinDate = DateTime.UtcNow.AddDays(-90),
    Status = "Inactive",
    CreatedAt = DateTime.UtcNow.AddDays(-90),
    UpdatedAt = DateTime.UtcNow.AddDays(-90)
},
new Employee
{
    Id = 4,
    FirstName = "Neha",
    LastName = "Kapoor",
    Email = "nehda.kapoor34@example.com",
    Phone = "9090909090",
    Department = "Marketing",
    Designation = "Marketing Executive",
    Salary = 45000,
    JoinDate = DateTime.UtcNow.AddDays(-60),
    Status = "Active",
    CreatedAt = DateTime.UtcNow.AddDays(-60),
    UpdatedAt = DateTime.UtcNow.AddDays(-60)
},
new Employee
{
    Id = 5,
    FirstName = "Suresh",
    LastName = "Patil",
    Email = "sueresh.patil35@example.com",
    Phone = "8888888888",
    Department = "Operations",
    Designation = "Operations Manager",
    Salary = 70000,
    JoinDate = DateTime.UtcNow.AddDays(-300),
    Status = "Active",
    CreatedAt = DateTime.UtcNow.AddDays(-300),
    UpdatedAt = DateTime.UtcNow.AddDays(-300)
},
new Employee
{
    Id = 6,
    FirstName = "Amit",
    LastName = "Sharma",
    Email = "amfit.sharma36@example.com",
    Phone = "9876543210",
    Department = "Engineering",
    Designation = "Software Engineer",
    Salary = 60000,
    JoinDate = new DateTime(2025, 01, 10),
    Status = "Active",
    CreatedAt = new DateTime(2025, 01, 10),
    UpdatedAt = new DateTime(2025, 01, 10)
},
new Employee
{
    Id = 7,
    FirstName = "Priya",
    LastName = "Verma",
    Email = "prgiya.verma37@example.com",
    Phone = "9876543211",
    Department = "Marketing",
    Designation = "Marketing Executive",
    Salary = 45000,
    JoinDate = new DateTime(2025, 02, 15),
    Status = "Active",
    CreatedAt = new DateTime(2025, 02, 15),
    UpdatedAt = new DateTime(2025, 02, 15)
},
new Employee
{
    Id = 8,
    FirstName = "Rahul",
    LastName = "Kuhmar",
    Email = "rahhul.kumar38@example.com",
    Phone = "9876543212",
    Department = "HR",
    Designation = "HR Manager",
    Salary = 70000,
    JoinDate = new DateTime(2024, 12, 05),
    Status = "Active",
    CreatedAt = new DateTime(2024, 12, 05),
    UpdatedAt = new DateTime(2024, 12, 05)
},
new Employee
{
    Id = 9,
    FirstName = "Sneha",
    LastName = "Patil",
    Email = "snehia.patil39@example.com",
    Phone = "9876543213",
    Department = "Finance",
    Designation = "Accountant",
    Salary = 52000,
    JoinDate = new DateTime(2024, 11, 20),
    Status = "Inactive",
    CreatedAt = new DateTime(2024, 11, 20),
    UpdatedAt = new DateTime(2024, 11, 20)
},
new Employee
{
    Id = 10,
    FirstName = "Vikram",
    LastName = "Singh",
    Email = "vikrjam.singh40@example.com",
    Phone = "9876543214",
    Department = "Operations",
    Designation = "Operations Manager",
    Salary = 80000,
    JoinDate = new DateTime(2024, 10, 12),
    Status = "Active",
    CreatedAt = new DateTime(2024, 10, 12),
    UpdatedAt = new DateTime(2024, 10, 12)
},
new Employee
{
    Id = 11,
    FirstName = "Neha",
    LastName = "Gupta",
    Email = "nehka.gupta41@example.com",
    Phone = "9876543215",
    Department = "Engineering",
    Designation = "Backend Developer",
    Salary = 65000,
    JoinDate = new DateTime(2025, 03, 01),
    Status = "Active",
    CreatedAt = new DateTime(2025, 03, 01),
    UpdatedAt = new DateTime(2025, 03, 01)
},
new Employee
{
    Id = 12,
    FirstName = "Arjun",
    LastName = "Mehta",
    Email = "arljun.mehta42@example.com",
    Phone = "9876543216",
    Department = "Engineering",
    Designation = "Frontend Developer",
    Salary = 62000,
    JoinDate = new DateTime(2025, 01, 25),
    Status = "Inactive",
    CreatedAt = new DateTime(2025, 01, 25),
    UpdatedAt = new DateTime(2025, 01, 25)
},
new Employee
{
    Id = 13,
    FirstName = "Kavya",
    LastName = "Nair",
    Email = "kavmya.nair43@example.com",
    Phone = "9876543217",
    Department = "Marketing",
    Designation = "SEO Analyst",
    Salary = 48000,
    JoinDate = new DateTime(2024, 09, 18),
    Status = "Active",
    CreatedAt = new DateTime(2024, 09, 18),
    UpdatedAt = new DateTime(2024, 09, 18)
},
new Employee
{
    Id = 14,
    FirstName = "Rohit",
    LastName = "Yadav",
    Email = "ronhit.yadav44@example.com",
    Phone = "9876543218",
    Department = "HR",
    Designation = "Recruiter",
    Salary = 43000,
    JoinDate = new DateTime(2024, 08, 30),
    Status = "Active",
    CreatedAt = new DateTime(2024, 08, 30),
    UpdatedAt = new DateTime(2024, 08, 30)
},
new Employee
{
    Id = 15,
    FirstName = "Anjali",
    LastName = "Deshmukh",
    Email = "anjoali.deshmukh45@example.com",
    Phone = "9876543219",
    Department = "Finance",
    Designation = "Financial Analyst",
    Salary = 55000,
    JoinDate = new DateTime(2024, 07, 10),
    Status = "Inactive",
    CreatedAt = new DateTime(2024, 07, 10),
    UpdatedAt = new DateTime(2024, 07, 10)
}
            );
        }
    }
}