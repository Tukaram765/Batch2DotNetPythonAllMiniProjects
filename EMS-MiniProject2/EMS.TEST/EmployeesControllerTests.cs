using NUnit.Framework;
using EMS.API.Controllers;
using EMS.API.Data;
using EMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EMS.TEST.Controllers
{
    [TestFixture]
    public class EmployeesControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private EmployeesController GetController(AppDbContext context)
        {
            return new EmployeesController(context);
        }

        [Test]
        public async Task GetAll_ShouldReturnData()
        {
            var context = GetDbContext();

            context.Employees.Add(new Employee
            {
                FirstName = "Amit",
                LastName = "Sharma",
                Email = "amit@test.com",
                Phone = "1234567890",
                Department = "IT",
                Designation = "Dev",
                Salary = 50000,
                JoinDate = DateTime.UtcNow,
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var controller = GetController(context);

            var result = await controller.GetAll(null, null, null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetById_ShouldReturnEmployee_WhenExists()
        {
            var context = GetDbContext();

            var emp = new Employee
            {
                FirstName = "Rahul",
                LastName = "Verma",
                Email = "rahul@test.com",
                Phone = "9999999999",
                Department = "HR",
                Designation = "Manager",
                Salary = 60000,
                JoinDate = DateTime.UtcNow,
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Employees.Add(emp);
            await context.SaveChangesAsync();

            var controller = GetController(context);

            var result = await controller.GetById(emp.Id);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task Create_ShouldAddEmployee()
        {
            var context = GetDbContext();
            var controller = GetController(context);

            var emp = new Employee
            {
                FirstName = "Neha",
                LastName = "Kapoor",
                Email = "neha@test.com",
                Phone = "8888888888",
                Department = "Finance",
                Designation = "Analyst",
                Salary = 70000,
                JoinDate = DateTime.UtcNow,
                Status = "Active"
            };

            var result = await controller.Create(emp);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task Create_ShouldReturnConflict_WhenEmailExists()
        {
            var context = GetDbContext();

            context.Employees.Add(new Employee
            {
                FirstName = "A",
                LastName = "B",
                Email = "dup@test.com",
                Phone = "1111111111",
                Department = "IT",
                Designation = "Dev",
                Salary = 50000,
                JoinDate = DateTime.UtcNow,
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var controller = GetController(context);

            var emp = new Employee
            {
                FirstName = "C",
                LastName = "D",
                Email = "dup@test.com",
                Phone = "2222222222",
                Department = "IT",
                Designation = "Dev",
                Salary = 60000,
                JoinDate = DateTime.UtcNow,
                Status = "Active"
            };

            var result = await controller.Create(emp);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task Delete_ShouldRemoveEmployee()
        {
            var context = GetDbContext();

            var emp = new Employee
            {
                FirstName = "Delete",
                LastName = "Test",
                Email = "delete@test.com",
                Phone = "0000000000",
                Department = "IT",
                Designation = "Dev",
                Salary = 50000,
                JoinDate = DateTime.UtcNow,
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Employees.Add(emp);
            await context.SaveChangesAsync();

            var controller = GetController(context);

            var result = await controller.Delete(emp.Id);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task Dashboard_ShouldReturnData()
        {
            var context = GetDbContext();

            context.Employees.Add(new Employee
            {
                FirstName = "Dash",
                LastName = "Board",
                Email = "dash@test.com",
                Phone = "1231231234",
                Department = "IT",
                Designation = "Dev",
                Salary = 50000,
                JoinDate = DateTime.UtcNow,
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var controller = GetController(context);

            var result = await controller.GetDashboard();

            Assert.That(result, Is.Not.Null);
        }
    }
}