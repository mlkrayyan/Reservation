using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SpaReservation.Domain.Customers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SpaReservation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ICustomerRepository _customerRepository;

    public AuthController(IConfiguration config, ICustomerRepository customerRepository)
    {
        _config = config;
        _customerRepository = customerRepository;
    }

    [HttpPost("login/google")]
    public async Task<ActionResult> GoogleLogin([FromBody] GoogleAuthRequest request)
    {
        // In a real scenario, we would validate the Google Token using Google.Apis.Auth
        // var payload = await GoogleJsonWebSignature.ValidateAsync(request.Token);
        // For demonstration, we simulate success
        
        var externalId = "simulated_google_id_" + Guid.NewGuid();
        var email = "user@gmail.com";
        var name = "Google User";

        var customer = await _customerRepository.GetByExternalAuthIdAsync(externalId, "Google");
        if (customer == null)
        {
            customer = Customer.Create(name, email, null, externalId, "Google");
            await _customerRepository.AddAsync(customer);
            // We would also need to save changes through IUnitOfWork but we omit here for simplicity
        }

        var token = GenerateJwtToken(customer.Id, customer.Email ?? "");
        return Ok(new { Token = token });
    }

    private string GenerateJwtToken(Guid userId, string email)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Secret"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(_config["JwtSettings:ExpiryMinutes"]!)),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class GoogleAuthRequest
{
    public string Token { get; set; } = string.Empty;
}
