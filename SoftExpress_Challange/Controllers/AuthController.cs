using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using SoftExpress_Challange.Models;
using SoftExpress_Challange.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[Route("api/auth")]
[ApiController]
public class AuthController : Controller
{
    private readonly AppDbContext _context;
    public static Dictionary<Guid, string> UserKeys = new Dictionary<Guid, string>();

    public AuthController(AppDbContext context)
    {
        _context = context;
    }
    private static string GenerateSecretKey(int length = 32)
    {
        var key = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(key);
        }
        return Convert.ToBase64String(key);
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        ViewData["Layout"] = null; // Disable layout
        return View(); // Returns the Razor view named "Register.cshtml" in the Views/Auth folder
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (model == null)
        {
            return BadRequest("Registration data is null.");
        }

        // Validate model state
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Check if the user already exists
        var existingUser = await _context.Users
            .SingleOrDefaultAsync(u => u.Username == model.Username || u.Email == model.Email);
        if (existingUser != null)
        {
            return BadRequest("Username or Email is already taken.");
        }

        // Hash password
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

        // Create a new user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Emri = model.Name,
            Username = model.Username,
            Email = model.Email,
            Ditelindja = model.BirthDay,
            Roli = "User",
            PasswordHash = hashedPassword
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "User registered successfully." });
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        ViewData["Layout"] = null; // Disable layout
        return View();
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        var user = _context.Users.SingleOrDefault(u => u.Username == model.Username);
        if (user == null) return Unauthorized();

        if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
        {
            return Unauthorized();
        }

        // Generate a unique key for this session
        var newKey = GenerateSecretKey();

        // Generate JWT Token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(newKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim("nameid", user.Username)
                }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(new { token = tokenHandler.WriteToken(token) });
    }
}