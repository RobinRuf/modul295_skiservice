using Microsoft.AspNetCore.Mvc;
using SkiService.Models;
using SkiService.Models.dto;
using SkiService.Services;
using SkiService.Helpers;

namespace SkiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SkiServiceContext _context;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(SkiServiceContext context, ITokenService tokenService, ILogger<AuthController> logger)
        {
            _context = context;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] LoginDto loginDto)
        {
            var user = _context.Employees.FirstOrDefault(u => u.Username == loginDto.Username);

            if (user == null)
            {
                _logger.LogError("Benutzer wurde nicht gefunden. Ist er in der DB in der employees-Tabelle?");
                return Unauthorized("Benutzer nicht gefunden.");
            }

            // Hash the input password and compare
            var hashedPassword = HashingHelper.ConvertToSha256(loginDto.Password);
            if (hashedPassword != user.Password)
            {
                _logger.LogError("Falsches Password.");
                return Unauthorized("Falsches Passwort.");
            }

            string token = _tokenService.CreateToken(user.Username);
            return Ok(token);
        }
    }

}
