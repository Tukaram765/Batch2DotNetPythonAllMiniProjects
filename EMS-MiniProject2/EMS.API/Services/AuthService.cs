using Microsoft.EntityFrameworkCore;
using EMS.API.Data;
using EMS.API.Models;
using EMS.API.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EMS.API.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // =====================================================
        // REGISTER
        // =====================================================
        public async Task<AuthResponseDto> RegisterAsync(AuthRequestDto dto)
        {
            // Check duplicate username (case-insensitive)
            var exists = await _context.Users
                .AnyAsync(u => u.Username.ToLower() == dto.Username.ToLower());

            if (exists)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Username already exists"
                };
            }

            // Hash password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new AppUser
            {
                Username = dto.Username,
                PasswordHash = hashedPassword,
                Role = string.IsNullOrEmpty(dto.Role) ? "Viewer" : dto.Role,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                Success = true,
                Username = user.Username,
                Role = user.Role,
                Message = "User registered successfully"
            };
        }

        // =====================================================
        // LOGIN
        // =====================================================
        public async Task<AuthResponseDto> LoginAsync(AuthRequestDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() == dto.Username.ToLower());

            // Invalid credentials
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            // Generate JWT
            var token = GenerateToken(user);

            return new AuthResponseDto
            {
                Success = true,
                Username = user.Username,
                Role = user.Role,
                Token = token,
                Message = "Login successful"
            };
        }

        // =====================================================
        // GENERATE JWT TOKEN
        // =====================================================
        private string GenerateToken(AppUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var expiryHours = double.Parse(_config["Jwt:ExpiryHours"] ?? "8");

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expiryHours),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}