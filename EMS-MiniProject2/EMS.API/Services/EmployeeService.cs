using Microsoft.EntityFrameworkCore;
using EMS.API.Data;
using EMS.API.Models;
using EMS.API.DTOs;

namespace EMS.API.Services
{
    public class EmployeeService
    {
        private readonly AppDbContext _context;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        // =========================================================
        // GET ALL (Search + Filter + Sort + Pagination)
        // =========================================================
        public async Task<PagedResult<EmployeeResponseDto>> GetAllAsync(EmployeeQueryParams qp)
        {
            var query = _context.Employees.AsQueryable();

            //  SEARCH (Name + Email)
            if (!string.IsNullOrWhiteSpace(qp.Search))
            {
                var term = qp.Search.ToLower();

                query = query.Where(e =>
                    (e.FirstName + " " + e.LastName).ToLower().Contains(term) ||
                    e.Email.ToLower().Contains(term));
            }

            // FILTER
            if (!string.IsNullOrEmpty(qp.Department))
                query = query.Where(e => e.Department == qp.Department);

            if (!string.IsNullOrEmpty(qp.Status))
                query = query.Where(e => e.Status == qp.Status);

            // SORT
            query = qp.SortBy.ToLower() switch
            {
                "salary" => qp.SortDir == "desc"
                    ? query.OrderByDescending(e => e.Salary)
                    : query.OrderBy(e => e.Salary),

                "joindate" => qp.SortDir == "desc"
                    ? query.OrderByDescending(e => e.JoinDate)
                    : query.OrderBy(e => e.JoinDate),

                _ => qp.SortDir == "desc"
                    ? query.OrderByDescending(e => e.LastName)
                           .ThenByDescending(e => e.FirstName)
                    : query.OrderBy(e => e.LastName)
                           .ThenBy(e => e.FirstName)
            };

            // TOTAL COUNT
            var totalCount = await query.CountAsync();

            // PAGINATION (SQL LEVEL)
            var employees = await query
                .Skip((qp.Page - 1) * qp.PageSize)
                .Take(qp.PageSize)
                .ToListAsync();

            // MAP TO DTO
            var data = employees.Select(MapToResponseDto).ToList();

            return new PagedResult<EmployeeResponseDto>
            {
                Data = data,
                TotalCount = totalCount,
                Page = qp.Page,
                PageSize = qp.PageSize
            };
        }

        // =========================================================
        // GET BY ID
        // =========================================================
        public async Task<EmployeeResponseDto?> GetByIdAsync(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            return emp == null ? null : MapToResponseDto(emp);
        }

        // =========================================================
        // CREATE
        // =========================================================
        public async Task<(bool Success, string? Error, EmployeeResponseDto? Data)> CreateAsync(EmployeeRequestDto dto)
        {
            // UNIQUE EMAIL CHECK
            var exists = await _context.Employees
                .AnyAsync(e => e.Email == dto.Email);

            if (exists)
                return (false, "Email already exists", null);

            var emp = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Department = dto.Department,
                Designation = dto.Designation,
                Salary = dto.Salary,
                JoinDate = dto.JoinDate,
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Employees.Add(emp);
            await _context.SaveChangesAsync();

            return (true, null, MapToResponseDto(emp));
        }

        // =========================================================
        //  UPDATE
        // =========================================================
        public async Task<(bool Success, string? Error, EmployeeResponseDto? Data)> UpdateAsync(int id, EmployeeRequestDto dto)
        {
            var emp = await _context.Employees.FindAsync(id);

            if (emp == null)
                return (false, "Employee not found", null);

            // UNIQUE EMAIL (exclude current)
            var exists = await _context.Employees
                .AnyAsync(e => e.Email == dto.Email && e.Id != id);

            if (exists)
                return (false, "Email already exists", null);

            emp.FirstName = dto.FirstName;
            emp.LastName = dto.LastName;
            emp.Email = dto.Email;
            emp.Phone = dto.Phone;
            emp.Department = dto.Department;
            emp.Designation = dto.Designation;
            emp.Salary = dto.Salary;
            emp.JoinDate = dto.JoinDate;
            emp.Status = dto.Status;
            emp.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return (true, null, MapToResponseDto(emp));
        }

        // =========================================================
        // DELETE
        // =========================================================
        public async Task<bool> DeleteAsync(int id)
        {
            var emp = await _context.Employees.FindAsync(id);

            if (emp == null)
                return false;

            _context.Employees.Remove(emp);
            await _context.SaveChangesAsync();

            return true;
        }

        // =========================================================
        // DASHBOARD
        // =========================================================
        public async Task<DashboardSummaryDto> GetDashboardAsync()
        {
            var total = await _context.Employees.CountAsync();
            var active = await _context.Employees.CountAsync(e => e.Status == "Active");
            var inactive = await _context.Employees.CountAsync(e => e.Status == "Inactive");

            var departments = await _context.Employees
                .GroupBy(e => e.Department)
                .Select(g => new DepartmentBreakdownDto
                {
                    Department = g.Key,
                    Count = g.Count(),
                    Percentage = 0
                })
                .OrderBy(d => d.Department)
                .ToListAsync();

            // Percentage calculation
            foreach (var d in departments)
            {
                d.Percentage = total == 0 ? 0 : (d.Count * 100.0) / total;
            }

            var recent = await _context.Employees
                .OrderByDescending(e => e.CreatedAt)
                .ThenByDescending(e => e.Id)
                .Take(5)
                .ToListAsync();

            return new DashboardSummaryDto
            {
                TotalEmployees = total,
                Active = active,
                Inactive = inactive,
                TotalDepartments = departments.Count,
                DepartmentBreakdown = departments,
                RecentEmployees = recent.Select(MapToResponseDto).ToList()
            };
        }

        // =========================================================
        // MAPPING
        // =========================================================
        private static EmployeeResponseDto MapToResponseDto(Employee e)
        {
            return new EmployeeResponseDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                Department = e.Department,
                Designation = e.Designation,
                Salary = e.Salary,
                JoinDate = e.JoinDate,
                Status = e.Status,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            };
        }
    }
}