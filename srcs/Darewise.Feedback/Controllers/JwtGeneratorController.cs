using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Darewise.Feedback.Controllers
{
    [ApiController]
    [Route("/jwt-fake")]
    public class JwtGeneratorController : ControllerBase
    {
        private static Guid _userId = Guid.Parse("8DD0E473-74F2-4F1C-B215-64EA0A6BB61E");
        private readonly JwtSecurityOptions _jwtSecurityOptions;

        public JwtGeneratorController(JwtSecurityOptions jwtSecurityOptions)
        {
            _jwtSecurityOptions = jwtSecurityOptions;
        }

        private SecurityKey GetSigningKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecurityOptions.JwtPrivateKey));
        private const string SIGNING_KEY_ALGORITHM = SecurityAlgorithms.HmacSha256Signature;

        [HttpGet]
        public async Task<string> Get()
        {
            var handler = new JwtSecurityTokenHandler();

            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.NameIdentifier,_userId.ToString())
            });
            SecurityToken securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = "feedback-api",
                Audience = "feedback-api",
                Expires = DateTime.UtcNow + TimeSpan.FromHours(72), // JWT should be short-lifetimed normally
                SigningCredentials = new SigningCredentials(GetSigningKey, SIGNING_KEY_ALGORITHM)
            });
            return handler.WriteToken(securityToken);
        }

        [HttpGet("admin")]
        public async Task<string> GetAdmin()
        {
            var handler = new JwtSecurityTokenHandler();

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, "feedback-admin"),
                new(ClaimTypes.NameIdentifier,_userId.ToString())
            });
            SecurityToken securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = "feedback-api",
                Audience = "feedback-api",
                Expires = DateTime.UtcNow + TimeSpan.FromHours(72), // JWT should be short-lifetimed normally
                SigningCredentials = new SigningCredentials(GetSigningKey, SIGNING_KEY_ALGORITHM)
            });
            return handler.WriteToken(securityToken);
        }
    }
}