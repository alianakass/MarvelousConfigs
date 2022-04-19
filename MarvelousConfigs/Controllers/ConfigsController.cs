using AutoMapper;
using FluentValidation;
using Marvelous.Contracts.Endpoints;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ResponseModels;
using MarvelousConfigs.API.Extensions;
using MarvelousConfigs.API.Models;
using MarvelousConfigs.BLL.AuthRequestClient;
using MarvelousConfigs.BLL.Helper.Exceptions;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarvelousConfigs.API.Controllers
{
    [ApiController]
    [Route("api/configs")]
    public class ConfigsController : AdvanceController
    {
        private readonly IConfigsService _service;
        private readonly IMapper _map;
        private readonly ILogger<ConfigsController> _logger;
        private readonly IAuthRequestClient _auth;
        private readonly IValidator<ConfigInputModel> _validator;

        public ConfigsController(IMapper mapper, IConfigsService service,
            ILogger<ConfigsController> logger, IAuthRequestClient auth, IValidator<ConfigInputModel> validator) : base(auth, logger)
        {
            _map = mapper;
            _service = service;
            _logger = logger;
            _auth = auth;
            _validator = validator;
        }

        //api/configs
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [SwaggerOperation("Add config")]
        public async Task<ActionResult<int>> AddConfig([FromBody] ConfigInputModel model)
        {
            await this.CheckRole(Role.Admin);
            _logger.LogInformation($"Request to add new config");
            int id = await _service.AddConfig(_map.Map<ConfigModel>(model));
            _logger.LogInformation($"Response to a request for add new config id {id}");
            return StatusCode(StatusCodes.Status201Created, id);
        }

        //api/configs/42
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [SwaggerOperation("Delete config by id")]
        public async Task<ActionResult> DeleteConfigById(int id)
        {
            await this.CheckRole(Role.Admin);
            _logger.LogInformation($"Request to delete config by id{id}");
            await _service.DeleteConfigById(id);
            _logger.LogInformation($"Response to a request for delete config by id{id}");
            return NoContent();
        }

        //api/configs/42
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [SwaggerOperation("Restore config by id")]
        public async Task<ActionResult> RestoreConfigById(int id)
        {
            await this.CheckRole(Role.Admin);
            _logger.LogInformation($"Request to restore config by id{id}");
            await _service.RestoreConfigById(id);
            _logger.LogInformation($"Response to a request for restore config by id{id}");
            return NoContent();
        }

        //api/configs
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [SwaggerOperation("Get all configs")]
        public async Task<ActionResult<List<ConfigOutputModel>>> GetAllConfigs()
        {
            await this.CheckRole(Role.Admin);
            _logger.LogInformation($"Request to get all configs");
            var configs = _map.Map<List<ConfigOutputModel>>(await _service.GetAllConfigs());
            _logger.LogInformation($"Response to a request for all configs");
            return Ok(configs);
        }

        //api/configs/42
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [SwaggerOperation("Update config by id")]
        public async Task<ActionResult> UpdateConfigById(int id, [FromBody] ConfigInputModel model)
        {
            await CheckRole(Role.Admin);

            if (model == null)
                throw new Exception("You must specify the table details in the request body");
            var validationResult = _validator.Validate(model);
            if (!validationResult.IsValid)
            {
                var ex = new ValidationException(validationResult.Errors);
                _logger.LogError(ex, ex.Message);
                throw ex;
            }

            _logger.LogInformation($"Request to update config by id{id}");
            await _service.UpdateConfigById(id, _map.Map<ConfigModel>(model));
            _logger.LogInformation($"Response to a request for update config by id{id}");
            return NoContent();
        }

        //api/configs/service/42
        [HttpGet("service/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [SwaggerOperation("Get configs by service id")]
        public async Task<ActionResult<List<ConfigOutputModel>>> GetConfigsByServiceId(int id)
        {
            await this.CheckRole(Role.Admin);
            _logger.LogInformation($"Request to get configs by service id{id}");
            var configs = _map.Map<List<ConfigOutputModel>>(await _service.GetConfigsByServiceId(id));
            _logger.LogInformation($"Response to a request for get configs by service id{id}");
            return Ok(configs);
        }

        //api/configs/by-service
        [HttpGet(ConfigsEndpoints.Configs)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [SwaggerOperation("Get configs by service address")]
        public async Task<ActionResult<List<ConfigResponseModel>>> GetConfigsByService()
        {
            _logger.LogInformation($"Request to get configs by service");
            var token = HttpContext.Request.Headers.Authorization.FirstOrDefault();
            if (token == null)
                throw new UnauthorizedException($"Request attempt from unauthorized user");
            string? name = HttpContext.Request.Headers[nameof(Microservice)][0];
            _logger.LogInformation($"Call belongs to the service {$"{name}"}");
            List<ConfigResponseModel>? configs = _map.Map<List<ConfigResponseModel>>(await _service.GetConfigsByService(token, name));
            _logger.LogInformation($"Response to a request for get configs by service {name}");
            return Ok(configs);
        }

    }
}
