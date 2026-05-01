using EMS.API.Models;

namespace EMS.API.Services
{
    public interface IEmployeeRepository
    {
        // BASE QUERY (for IQueryable operations)
        IQueryable<Employee> Query();

        // GET BY ID
        Task<Employee?> GetByIdAsync(int id);

        //  CREATE
        Task AddAsync(Employee employee);

        // UPDATE (tracked entity)
        Task UpdateAsync(Employee employee);

        // DELETE
        Task DeleteAsync(Employee employee);

        // SAVE CHANGES
        Task SaveChangesAsync();

        // CHECK EMAIL EXISTS
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);

        // DASHBOARD SUPPORT
        Task<int> CountAsync();

        Task<int> CountByStatusAsync(string status);

        Task<List<(string Department, int Count)>> GetDepartmentCountsAsync();

        Task<List<Employee>> GetRecentAsync(int count);
    }
}