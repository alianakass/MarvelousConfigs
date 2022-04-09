using Marvelous.Contracts.Enums;
using MarvelousConfigs.BLL.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MarvelousConfigs.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;

        public AuthService(ILogger<AuthService> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetToken(string email, string pass)
        {
            if (email == "string" && pass == "string")
            {
                var claims = new List<Claim> {
                new Claim(ClaimTypes.Role, Role.Admin.ToString() )
                };

                var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.Issuer,
                        audience: AuthOptions.Audience,
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)), // время действия 30 минут
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                        SecurityAlgorithms.HmacSha256));
                return new JwtSecurityTokenHandler().WriteToken(jwt);
            }
            else
            {
                throw new Exception();
            }
        }

    }
}
