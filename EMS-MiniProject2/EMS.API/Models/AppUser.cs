using System.ComponentModel.DataAnnotations;

namespace EMS.API.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }  

        public string? Role { get; set; }   

        public DateTime CreatedAt { get; set; }
    }
}