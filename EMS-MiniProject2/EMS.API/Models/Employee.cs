using System.ComponentModel.DataAnnotations;

namespace EMS.API.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Required, MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(15)]
        public string Phone { get; set; }

        [Required, MaxLength(50)]
        public string Department { get; set; }

        [Required, MaxLength(100)]
        public string Designation { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Salary must be positive")]
        public decimal Salary { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        [Required, MaxLength(10)]
        public string Status { get; set; }  

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}