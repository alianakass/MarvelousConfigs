using AutoMapper;
using MarvelousConfigs.API.Attributes;
using MarvelousConfigs.API.Models;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarvelousConfigs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MicroservicesController : ControllerBase
    {
        private readonly IMicroservicesService _service;
        private readonly IMapper _map;
        private readonly ILogger _logger;

        public MicroservicesController(IMapper mapper, IMicroservicesService service, ILogger<MicroservicesController> logger)
        {
            _map = mapper;
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [SwaggerResponse(200, "OK", typeof(MicroserviceResponceModel))]
        [SwaggerResponse(400, "Bad Request", typeof(ExceptionResponse))]
        [SwaggerResponse(404, "NotFound", typeof(ExceptionResponse))]
        public async Task<ActionResult<int>> AddMicroservice([FromBody] MicroserviceInputModel model)
        {
            _logger.LogInformation($"Request to add new microservice");
            int id = await _service.AddMicroservice(_map.Map<MicroserviceModel>(model));
            _logger.LogInformation($"Response to a request for add new microservice");
            return Ok(id);
        }

        [HttpDelete("{id}")]
        [SwaggerResponse(204, "NoContent")]
        [SwaggerResponse(400, "Bad Request", typeof(ExceptionResponse))]
        [SwaggerResponse(404, "NotFound", typeof(ExceptionResponse))]
        public async Task<ActionResult> DeleteMicroserviceById(int id)
        {
            _logger.LogInformation($"Request to delete microservice by id{id}");
            await _service.DeleteMicroservice(id);
            _logger.LogInformation($"Response to a request for delete microservice by id{id}");
            return NoContent();
        }

        [HttpPatch("{id}")]
        [SwaggerResponse(204, "NoContent")]
        [SwaggerResponse(400, "Bad Request", typeof(ExceptionResponse))]
        [SwaggerResponse(404, "NotFound", typeof(ExceptionResponse))]
        public async Task<ActionResult> RestoreMicroserviceById(int id)
        {
            _logger.LogInformation($"Request to restore microservice by id{id}");
            await _service.RestoreMicroservice(id);
            _logger.LogInformation($"Response to a request for restore microservice by id{id}");
            return NoContent();
        }

        [HttpGet]
        [SwaggerResponse(200, "OK", typeof(MicroserviceResponceModel))]
        [SwaggerResponse(400, "Bad Request", typeof(ExceptionResponse))]
        public async Task<ActionResult<List<MicroserviceResponceModel>>> GetAllMicroservices()
        {
            _logger.LogInformation($"Request to get all microservices");
            var services = _map.Map<List<MicroserviceResponceModel>>(await _service.GetAllMicroservices());
            _logger.LogInformation($"Response to a request for get all microservices");
            return Ok(services);
        }

        [HttpPut("{id}")]
        [SwaggerResponse(204, "NoContent")]
        [SwaggerResponse(400, "Bad Request", typeof(ExceptionResponse))]
        [SwaggerResponse(404, "NotFound", typeof(ExceptionResponse))]
        public async Task<ActionResult<List<MicroserviceResponceModel>>> UpdateMicroserviceById(int id, [FromBody] MicroserviceInputModel model)
        {
            _logger.LogInformation($"Request to update microservice by id{id}");
            await _service.UpdateMicroservice(id, _map.Map<MicroserviceModel>(model));
            _logger.LogInformation($"Response to a request for update microservice by id{id}");
            return NoContent();
        }

        [HttpGet("with-configs")]
        [SwaggerResponse(200, "OK", typeof(MicroserviceWithConfigsResponceModel))]
        [SwaggerResponse(400, "Bad Request", typeof(ExceptionResponse))]
        public async Task<ActionResult<List<MicroserviceWithConfigsResponceModel>>> GetAllMicroservicesWithConfigs()
        {
            _logger.LogInformation($"Request to get all microservices with configs");
            var services = _map.Map<List<MicroserviceWithConfigsResponceModel>>(await _service.GetAllMicroservicesWithConfigs());
            _logger.LogInformation($"Response to a request for get all microservices with configs");
            return Ok(services);
        }

        [HttpGet("{id}")]
        [SwaggerResponse(200, "OK", typeof(MicroserviceWithConfigsResponceModel))]
        [SwaggerResponse(400, "Bad Request", typeof(ExceptionResponse))]
        [SwaggerResponse(404, "NotFound", typeof(ExceptionResponse))]
        public async Task<ActionResult<MicroserviceWithConfigsResponceModel>> GetMicroserviceWithConfigsById(int id)
        {
            _logger.LogInformation($"Request to get microservice with configs by id{id}");
            var services = _map.Map<MicroserviceWithConfigsResponceModel>(await _service.GetMicroserviceWithConfigsById(id));
            _logger.LogInformation($"Response to a request for get microservice with configs by id{id}");
            return Ok(services);
        }
    }
}
