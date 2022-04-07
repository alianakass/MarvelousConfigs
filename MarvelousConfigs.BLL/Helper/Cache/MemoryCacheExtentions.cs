using MarvelousConfigs.DAL.Entities;
using MarvelousConfigs.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace MarvelousConfigs.BLL.Cache
{
    public class MemoryCacheExtentions : IMemoryCacheExtentions
    {
        private readonly IMemoryCache _cache;
        private readonly IConfigsRepository _config;
        private readonly IMicroserviceRepository _microservice;

        public MemoryCacheExtentions(IMemoryCache cache, IMicroserviceRepository microservice,
            IConfigsRepository configs)
        {
            _cache = cache;
            _microservice = microservice;
            _config = configs;
        }

        public void SetMemoryCache()
        {
            var services = _microservice.GetAllMicroservices().Result;
            foreach (var c in services)
            {
                _cache.Set((Marvelous.Contracts.Enums.Microservice)c.Id, c);
            }

            var configs = _config.GetAllConfigs().Result;
            foreach (var config in configs)
            {
                _cache.Set(config.Id, config);
            }

            foreach (var s in services)
            {
                List<Config> cfgs = new List<Config>();
                foreach (var c in configs)
                {
                    if (s.Id == c.ServiceId)
                    {
                        cfgs.Add(c);
                    }
                }
                _cache.Set(s.Address, cfgs);
            }
        }
    }
}
