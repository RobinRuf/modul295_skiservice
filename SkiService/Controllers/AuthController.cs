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

            // Check if account is locked
            if (user.IsLocked)
            {
                _logger.LogError("Konto gesperrt aufgrund zu vieler Fehlanmeldeversuche.");
                return Unauthorized("Konto gesperrt.");
            }

            var hashedPassword = HashingHelper.ConvertToSha256(loginDto.Password);

            if (hashedPassword != user.Password)
            {
                user.LoginAttempts += 1;

                // Lock account after 3 failed attempts
                if (user.LoginAttempts >= 3)
                {
                    user.IsLocked = true;
                    _logger.LogError("Konto wurde gesperrt aufgrund zu vieler Fehlanmeldeversuche.");
                }

                _context.SaveChanges();
                _logger.LogError("Falsches Passwort.");
                return Unauthorized("Falsches Passwort.");
            }

            // Reset login attempts after successful login
            user.LoginAttempts = 0;
            _context.SaveChanges();

            string token = _tokenService.CreateToken(user.Username);
            return Ok(token);
        }
    }

}
