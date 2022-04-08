using Marvelous.Contracts.RequestModels;
using MarvelousConfigs.API.Models;
using MarvelousConfigs.BLL.AuthRequestClient;
using MarvelousConfigs.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarvelousConfigs.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IAuthRequestClient _auth;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthRequestClient auth, ILogger<AuthController> logger)
        {
            _auth = auth;
            _logger = logger;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation("Authentication")]
        public async Task<ActionResult<string>> Login([FromBody] AuthRequestModel auth)
        {
            _logger.LogInformation($"Trying to login with email {auth.Email}");
            string token = await _auth.GetToken(auth);
            _logger.LogInformation($"Admin with email {auth.Email} successfully logged in");
            return Ok(token);
        }
    }
}

