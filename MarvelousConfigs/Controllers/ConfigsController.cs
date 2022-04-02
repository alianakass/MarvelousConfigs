using AutoMapper;
using MarvelousConfigs.API.Attributes;
using MarvelousConfigs.API.Models;
using MarvelousConfigs.BLL.Cache;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarvelousConfigs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigsController : ControllerBase
    {
        private readonly IConfigsService _service;
        private readonly IMapper _map;
        private readonly ILogger<ConfigsController> _logger;
        public ConfigsController(IMapper mapper, IConfigsService service, ILogger<ConfigsController> logger, IConfigCache cache)
        {
            _map = mapper;
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [SwaggerResponse(200, "OK", typeof(MicroserviceWithConfigsResponceModel))]
        [SwaggerResponse(400, "Bad Request", typeof(ExceptionResponse))]
        [SwaggerResponse(404, "NotFound", typeof(ExceptionResponse))]
        public async Task<ActionResult<int>> AddConfig([FromBody] ConfigInputModel model)
        {
            _logger.LogInformation($"Request to add new config");
            int id = await _service.AddConfig(_map.Map<ConfigModel>(model));
            _logger.LogInformation($"Response to a request for add new config id {id}");
            return Ok(id);
        }

        [HttpDelete("{id}")]
        [SwaggerResponse(204, "NoContent")]
        [SwaggerResponse(400, "Bad Request", typeof(ExceptionResponse))]
        [SwaggerResponse(404, "NotFound", typeof(ExceptionResponse))]
        public async Task<ActionResult> DeleteConfigById(int id)
        {
            _logger.LogInformation($"Request to delete config by id{id}");
            await _service.DeleteConfigById(id);
            _logger.LogInformation($"Response to a request for delete config by id{id}");
            return NoContent();
        }

        [HttpPatch("{id}")]
        [SwaggerResponse(204, "NoContent")]
        [SwaggerResponse(400, "Bad Request", typeof(ExceptionResponse))]
        [SwaggerResponse(404, "NotFound", typeof(ExceptionResponse))]
        public async Task<ActionResult> RestoreConfigById(int id)
        {
            _logger.LogInformation($"Request to restore config by id{id}");
            await _service.RestoreConfigById(id);
            _logger.LogInformation($"Response to a request for restore config by id{id}");
            return NoContent();
        }

        [HttpGet]
        [SwaggerResponse(200, "OK", typeof(MicroserviceWithConfigsResponceModel))]
        [SwaggerResponse(400, "Bad Request", typeof(ExceptionResponse))]
        public async Task<ActionResult<List<ConfigResponceModel>>> GetAllConfigs()
        {
            _logger.LogInformation($"Request to get all configs");
            var configs = await _service.GetAllConfigs();
            _logger.LogInformation($"Response to a request for all configs");
            return Ok(configs);
        }

        [HttpPut("{id}")]
        [SwaggerResponse(204, "NoContent")]
        [SwaggerResponse(400, "Bad Request", typeof(ExceptionResponse))]
        [SwaggerResponse(404, "NotFound", typeof(ExceptionResponse))]
        public async Task<ActionResult<List<ConfigResponceModel>>> UpdateConfigById(int id, [FromBody] ConfigInputModel model)
        {
            _logger.LogInformation($"Request to update config by id{id}");
            await _service.UpdateConfigById(id, _map.Map<ConfigModel>(model));
            _logger.LogInformation($"Response to a request for update config by id{id}");
            return NoContent();
        }

    }
}
