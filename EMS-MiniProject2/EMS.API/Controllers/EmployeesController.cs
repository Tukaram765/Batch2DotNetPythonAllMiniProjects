using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMS.API.Data;
using EMS.API.Models;

namespace EMS.API.Controllers
{
    [ApiController]
    [Route("api/employees")]
    [Authorize(Roles = "Admin,Viewer")]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // api/employees (Search + Filter + Sort + Pagination)
        [HttpGet]
        public async Task<IActionResult> GetAll(
            string? search,
            string? department,
            string? status,
            string sortBy = "name",
            string sortDir = "asc",
            int page = 1,
            int pageSize = 10)
        {
            if (pageSize > 100) pageSize = 100;
            if (page < 1) page = 1;

            var query = _context.Employees.AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(e =>
                    (e.FirstName + " " + e.LastName).ToLower().Contains(search) ||
                    e.Email.ToLower().Contains(search));
            }

            // Filter
            if (!string.IsNullOrEmpty(department))
                query = query.Where(e => e.Department == department);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(e => e.Status == status);

            // Sort
            query = sortBy.ToLower() switch
            {
                "salary" => sortDir == "desc"
                    ? query.OrderByDescending(e => e.Salary)
                    : query.OrderBy(e => e.Salary),

                "joindate" => sortDir == "desc"
                    ? query.OrderByDescending(e => e.JoinDate)
                    : query.OrderBy(e => e.JoinDate),

                _ => sortDir == "desc"
                    ? query.OrderByDescending(e => e.LastName).ThenByDescending(e => e.FirstName)
                    : query.OrderBy(e => e.LastName).ThenBy(e => e.FirstName)
            };

            // Total Count
            var totalCount = await query.CountAsync();

            // Pagination
            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new
            {
                data,
                totalCount,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                hasNextPage = page * pageSize < totalCount,
                hasPrevPage = page > 1
            };

            return Ok(result);
        }

        // api/employees/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var emp = await _context.Employees.FindAsync(id);

            if (emp == null)
                return NotFound();

            return Ok(emp);
        }

        // api/employees (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Employee emp)
        {
            if (emp == null)
                return BadRequest(new { message = "Invalid employee data" });

            // Duplicate Email Check
            var exists = await _context.Employees
                .AnyAsync(e => e.Email == emp.Email);

            if (exists)
                return Conflict(new { message = "Email already exists" });

            emp.CreatedAt = DateTime.UtcNow;
            emp.UpdatedAt = DateTime.UtcNow;

            _context.Employees.Add(emp);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = emp.Id }, emp);
        }

        // api/employees/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, Employee updated)
        {
            var emp = await _context.Employees.FindAsync(id);

            if (emp == null)
                return NotFound();

            //Email uniqueness (exclude current record)
            var exists = await _context.Employees
                .AnyAsync(e => e.Email == updated.Email && e.Id != id);

            if (exists)
                return Conflict(new { message = "Email already exists" });

            // Update fields
            emp.FirstName = updated.FirstName;
            emp.LastName = updated.LastName;
            emp.Email = updated.Email;
            emp.Phone = updated.Phone;
            emp.Department = updated.Department;
            emp.Designation = updated.Designation;
            emp.Salary = updated.Salary;
            emp.JoinDate = updated.JoinDate;
            emp.Status = updated.Status;
            emp.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(emp);
        }

        // api/employees/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var emp = await _context.Employees.FindAsync(id);

            if (emp == null)
                return NotFound();

            _context.Employees.Remove(emp);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Employee deleted successfully" });
        }

        // api/employees/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var totalEmployees = await _context.Employees.CountAsync();
            var active = await _context.Employees.CountAsync(e => e.Status == "Active");
            var inactive = await _context.Employees.CountAsync(e => e.Status == "Inactive");


            var departments = await _context.Employees
                .GroupBy(e => e.Department)
                .Select(g => new
                {
                    department = g.Key,
                    count = g.Count(),
                    percentage = totalEmployees == 0
                        ? 0
                        : Math.Round((g.Count() * 100.0) / totalEmployees, 2)
                })
                .OrderBy(d => d.department)
                .ToListAsync();

            var recent = await _context.Employees
                .OrderByDescending(e => e.CreatedAt)
                .ThenByDescending(e => e.Id)
                .Take(5)
                .ToListAsync();

            return Ok(new
            {
                totalEmployees,
                active,
                inactive,
                totalDepartments = departments.Count,
                departmentBreakdown = departments,
                recentEmployees = recent
            });
        }
    }
}