using Microsoft.Extensions.Logging;

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
            return "";
        }

    }
}
