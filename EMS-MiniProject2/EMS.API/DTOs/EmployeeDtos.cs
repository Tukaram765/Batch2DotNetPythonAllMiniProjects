using System.ComponentModel.DataAnnotations;

namespace EMS.API.DTOs
{
    // ================================
    // CREATE / UPDATE REQUEST DTO
    // ================================
    public class EmployeeRequestDto
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; }

        [Required, RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be 10 digits")]
        public string Phone { get; set; }

        [Required]
        public string Department { get; set; }  // Engineering, HR, etc.

        [Required, MaxLength(100)]
        public string Designation { get; set; }

        [Required, Range(1, double.MaxValue, ErrorMessage = "Salary must be positive")]
        public decimal Salary { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        [Required]
        [RegularExpression("Active|Inactive")]
        public string Status { get; set; }
    }


    // ================================
    //  RESPONSE DTO
    // ================================
    public class EmployeeResponseDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; }
        public string Phone { get; set; }

        public string Department { get; set; }
        public string Designation { get; set; }

        public decimal Salary { get; set; }

        public DateTime JoinDate { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }


    // ================================
    // PAGINATION RESULT DTO
    // ================================
    public class PagedResult<T>
    {
        public List<T> Data { get; set; } = new List<T>();

        public int TotalCount { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public bool HasNextPage => Page < TotalPages;

        public bool HasPrevPage => Page > 1;
    }


    // ================================
    //  QUERY PARAMETERS DTO
    // ================================
    public class EmployeeQueryParams
    {
        public string? Search { get; set; }

        public string? Department { get; set; }

        public string? Status { get; set; }

        public string SortBy { get; set; } = "name";   // name | salary | joinDate

        public string SortDir { get; set; } = "asc";   // asc | desc

        public int Page { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 100 ? 100 : value;
        }
    }


    // ================================
    // DASHBOARD DTO
    // ================================
    public class DashboardSummaryDto
    {
        public int TotalEmployees { get; set; }

        public int Active { get; set; }

        public int Inactive { get; set; }

        public int TotalDepartments { get; set; }

        public List<DepartmentBreakdownDto> DepartmentBreakdown { get; set; }

        public List<EmployeeResponseDto> RecentEmployees { get; set; }
    }

    public class DepartmentBreakdownDto
    {
        public string Department { get; set; }

        public int Count { get; set; }

        public double Percentage { get; set; }
    }


    // ================================
    // AUTH DTOs
    // ================================
    public class AuthRequestDto
    {
        [Required]
        public string Username { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        public string? Role { get; set; }  // optional for register
    }

    public class AuthResponseDto
    {
        public bool Success { get; set; }

        public string Username { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }

        public string Message { get; set; }
    }
}