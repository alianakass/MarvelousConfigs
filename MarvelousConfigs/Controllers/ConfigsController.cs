﻿using AutoMapper;
using Marvelous.Contracts.Enums;
using MarvelousConfigs.API.Attribute;
using MarvelousConfigs.API.Models;
using MarvelousConfigs.API.RMQ.Producers;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarvelousConfigs.API.Controllers
{
    [ApiController]
    [Route("api/configs")]
    public class ConfigsController : ControllerBase
    {
        private readonly IConfigsService _service;
        private readonly IMapper _map;
        private readonly ILogger<ConfigsController> _logger;
        private readonly IMarvelousConfigsProducer _prod;

        public ConfigsController(IMapper mapper, IConfigsService service,
            ILogger<ConfigsController> logger, IMarvelousConfigsProducer producer)
        {
            _map = mapper;
            _service = service;
            _logger = logger;
            _prod = producer;
        }

        //api/configs
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[AuthorizeEnum(Role.Admin)]
        [SwaggerOperation("Add config")]
        public async Task<ActionResult<int>> AddConfig([FromBody] ConfigInputModel model)
        {
            _logger.LogInformation($"Request to add new config");
            int id = await _service.AddConfig(_map.Map<ConfigModel>(model));
            _logger.LogInformation($"Response to a request for add new config id {id}");
            await _prod.NotifyConfigurationAddedOrUpdated(id);
            return StatusCode(StatusCodes.Status201Created, id);
        }

        //api/configs/42
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[AuthorizeEnum(Role.Admin)]
        [SwaggerOperation("Delete config by id")]
        public async Task<ActionResult> DeleteConfigById(int id)
        {
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
        //[AuthorizeEnum(Role.Admin)]
        [SwaggerOperation("Restore config by id")]
        public async Task<ActionResult> RestoreConfigById(int id)
        {
            _logger.LogInformation($"Request to restore config by id{id}");
            await _service.RestoreConfigById(id);
            await _prod.NotifyConfigurationAddedOrUpdated(id);
            _logger.LogInformation($"Response to a request for restore config by id{id}");
            return NoContent();
        }

        //api/configs
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[AuthorizeEnum(Role.Admin)]
        [SwaggerOperation("Get all configs")]
        public async Task<ActionResult<List<ConfigOutputModel>>> GetAllConfigs()
        {
            _logger.LogInformation($"Request to get all configs");
            var configs = _map.Map<List<MicroserviceOutputModel>>(await _service.GetAllConfigs());
            _logger.LogInformation($"Response to a request for all configs");
            return Ok(configs);
        }

        //api/configs/42
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[AuthorizeEnum(Role.Admin)]
        [SwaggerOperation("Update config by id")]
        public async Task<ActionResult> UpdateConfigById(int id, [FromBody] ConfigInputModel model)
        {
            _logger.LogInformation($"Request to update config by id{id}");
            await _service.UpdateConfigById(id, _map.Map<ConfigModel>(model));
            await _prod.NotifyConfigurationAddedOrUpdated(id);
            _logger.LogInformation($"Response to a request for update config by id{id}");
            return NoContent();
        }

        //api/configs/42
        [HttpGet("service/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[AuthorizeEnum(Role.Admin)]
        [SwaggerOperation("Get configs by service id")]
        public async Task<ActionResult<List<ConfigOutputModel>>> GetConfigsByServiceId(int id)
        {
            _logger.LogInformation($"Request to get configs by service id{id}");
            var configs = _map.Map<List<ConfigOutputModel>>(await _service.GetConfigsByServiceId(id));
            _logger.LogInformation($"Response to a request for get configs by service id{id}");
            return Ok(configs);
        }

        //api/configs/by-serviceAddress
        [HttpGet("by-serviceAddress")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("Get configs by service address")]
        public async Task<ActionResult<List<ConfigOutputModel>>> GetConfigsByServiceAddress()
        {
            _logger.LogInformation($"Request to get configs by service {"test data"}");
            var configs = _map.Map<List<ConfigOutputModel>>(await _service.GetConfigsByServiceAddress(""));
            _logger.LogInformation($"Response to a request for get configs by service {"test data"}");
            return Ok(configs);
        }

    }
}
