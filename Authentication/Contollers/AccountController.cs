using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authentication.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.Contollers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IConfiguration configuration): ControllerBase
{
    private static ConcurrentDictionary<string, string> UserData { get; set; }
        = new ConcurrentDictionary<string, string>(); 
    
    // api/account/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        await Task.Delay(500);
        
        var _email = UserData!.Keys
            .Where( e => e.Equals( request.Email)).FirstOrDefault();

        if (_email is null)
        {
            return NotFound("email not found");
        }
        else
        {
            UserData.TryGetValue(_email, out string? dbPassword);
            
            if (dbPassword is null || !dbPassword.Equals(request.Password))
            {
                return BadRequest("Email/password is incorrect");
            }
            else
            {
                string jwtToken = GenerateToken(_email);
                return Ok(jwtToken);
            }
        }
    }

    private string GenerateToken(string email)
    {
        var key = Encoding.UTF8.GetBytes(configuration["Authentication:Key"]);
        var securityKey = new SymmetricSecurityKey(key);
        var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[] { new Claim(ClaimTypes.Email, email!) };
        var token = new JwtSecurityToken(
            issuer: configuration["Authentication:Issuer"],
            audience: configuration["Authentication:Audience"],
            claims: claims,
            expires: null,
            signingCredentials: credential
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);  
    }

    // api/account/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        await Task.Delay(500);
        
        var _email = UserData!.Keys
            .Where( e => e.Equals( request.Email)).FirstOrDefault();

        if (_email is null)
        {
            UserData[request.Email] = request.Password;
            return Ok("User registered successfully"); 
        }
        else
        {
            return BadRequest("User already exists");
        }
    }
    
}