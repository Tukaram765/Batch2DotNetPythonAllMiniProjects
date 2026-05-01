using EMS.API.Data;
using EMS.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Text;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // =========================
    // REGISTER
    // =========================
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AppUser model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.PasswordHash))
        {
            return BadRequest(new { success = false, message = "Username and Password required" });
        }

        var exists = await _context.Users
            .AnyAsync(u => u.Username.ToLower() == model.Username.ToLower());

        if (exists)
            return Conflict(new { success = false, message = "Username already exists" });

        var user = new AppUser
        {
            Username = model.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash),
            Role = string.IsNullOrEmpty(model.Role) ? "Viewer" : model.Role,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Registered successfully" });
    }

    // =========================
    // LOGIN
    // =========================
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AppUser model)
    {
        if (model == null)
            return BadRequest(new { success = false, message = "Empty request body" });

        if (string.IsNullOrWhiteSpace(model.Username) ||
            string.IsNullOrWhiteSpace(model.PasswordHash))
        {
            return BadRequest(new { success = false, message = "Username & Password required" });
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username.ToLower() == model.Username.ToLower());

        if (user == null)
        {
            return Unauthorized(new { success = false, message = "User not found" });
        }

        bool valid = BCrypt.Net.BCrypt.Verify(model.PasswordHash, user.PasswordHash);

        if (!valid)
        {
            return Unauthorized(new { success = false, message = "Wrong password" });
        }

        return Ok(new
        {
            success = true,
            username = user.Username,
            role = user.Role,
            token = GenerateToken(user)
        });
    }

    // JWT
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

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}