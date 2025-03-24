using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ProjectManagement.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ProjectManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger; // Добавьте это поле

        public AccountController(UserManager<User> userManager, IConfiguration configuration, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (model == null)
            {
                _logger.LogError("Request body is null");
                return BadRequest("Request body is empty");
            }

            _logger.LogInformation($"Received data: {JsonSerializer.Serialize(model)}");

            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Email))
            {
                _logger.LogError("Username or email is empty");
                return BadRequest("Username and email are required");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Model validation failed: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Role = model.Role
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Ok(new { Message = "User registered successfully" });
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddDays(7),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return Unauthorized();
        }
    }
}