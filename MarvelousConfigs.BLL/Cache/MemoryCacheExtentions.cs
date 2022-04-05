using AutoMapper;
using Marvelous.Contracts.Enums;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace MarvelousConfigs.BLL.Cache
{
    public class MemoryCacheExtentions : IMemoryCacheExtentions
    {
        private readonly IMemoryCache _cache;
        private readonly IConfigsRepository _config;
        private readonly IMicroserviceRepository _microservice;
        private readonly IMapper _map;

        public MemoryCacheExtentions(IMemoryCache cache, IMicroserviceRepository microservice,
            IConfigsRepository configs, IMapper mapper)
        {
            _cache = cache;
            _microservice = microservice;
            _config = configs;
            _map = mapper;
        }

        public void SetMemoryCache()
        {
            var services = _map.Map<List<MicroserviceModel>>(_microservice.GetAllMicroservices().Result);
            foreach (var c in services)
            {
                _cache.Set((Microservice)c.Id, c);
            }

            var configs = _map.Map<List<ConfigModel>>(_config.GetAllConfigs().Result);
            foreach (var config in configs)
            {
                _cache.Set(config.Id, config);
            }
        }
    }
}
