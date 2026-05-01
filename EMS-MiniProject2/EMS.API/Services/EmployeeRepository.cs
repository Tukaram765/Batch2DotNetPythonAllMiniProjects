using Microsoft.EntityFrameworkCore;
using EMS.API.Data;
using EMS.API.Models;

namespace EMS.API.Services
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        // =========================================
        // BASE QUERY (IQueryable for filtering in service)
        // =========================================
        public IQueryable<Employee> Query()
        {
            return _context.Employees.AsQueryable();
        }

        // =========================================
        //  GET BY ID
        // =========================================
        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        // =========================================
        //  CREATE
        // =========================================
        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
        }

        // =========================================
        // UPDATE
        // =========================================
        public Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            return Task.CompletedTask;
        }

        // =========================================
        //  DELETE
        // =========================================
        public Task DeleteAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            return Task.CompletedTask;
        }

        // =========================================
        //  SAVE CHANGES
        // =========================================
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // =========================================
        // CHECK EMAIL EXISTS
        // =========================================
        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            return await _context.Employees
                .AnyAsync(e => e.Email == email && (!excludeId.HasValue || e.Id != excludeId));
        }

        // =========================================
        //  TOTAL COUNT
        // =========================================
        public async Task<int> CountAsync()
        {
            return await _context.Employees.CountAsync();
        }

        // =========================================
        //  COUNT BY STATUS
        // =========================================
        public async Task<int> CountByStatusAsync(string status)
        {
            return await _context.Employees
                .CountAsync(e => e.Status == status);
        }

        // =========================================
        // DEPARTMENT GROUPING
        // =========================================
        public async Task<List<(string Department, int Count)>> GetDepartmentCountsAsync()
        {
            var result = await _context.Employees
                .GroupBy(e => e.Department)
                .Select(g => new
                {
                    Department = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            return result
                .Select(r => (r.Department, r.Count))
                .ToList();
        }

        // =========================================
        // RECENT EMPLOYEES
        // =========================================
        public async Task<List<Employee>> GetRecentAsync(int count)
        {
            return await _context.Employees
                .OrderByDescending(e => e.CreatedAt)
                .ThenByDescending(e => e.Id)
                .Take(count)
                .ToListAsync();
        }
    }
}