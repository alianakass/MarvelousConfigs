using Marvelous.Contracts.RequestModels;
using MarvelousConfigs.API.Models;
using MarvelousConfigs.BLL.AuthRequestClient;
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
        
        private readonly ILogger<AuthController> _logger;


        public AuthController(ILogger<AuthController> logger)
        {
            
            _logger = logger;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation("Authentication")]
        public async Task<ActionResult> Login([FromBody] AdminLoginInputModel auth)
        {
            _logger.LogInformation($"Trying to login with email {auth.Email}");
            _logger.LogInformation($"Admin with email {auth.Email} successfully logged in");
            return Ok();
        }
    }
}

