using AutoMapper;
using MarvelousConfigs.BLL.Exeptions;
using MarvelousConfigs.BLL.Models;
using Microsoft.Extensions.Caching.Memory;
using static MarvelousConfigs.BLL.Services.ConfigsService;

namespace MarvelousConfigs.BLL.Cache
{
    public class ConfigCache : IConfigCache
    {
        private readonly IMemoryCache _cache;
        private readonly IMapper _map;

        public ConfigCache(IMemoryCache cache, IMapper mapper)
        {
            _cache = cache;
            _map = mapper;
        }

        public void SetCache(List<ConfigModel> configs)
        {
            foreach (ConfigModel config in configs)
            {
                MicroserviceModel microserviceModel = null;
                if (!_cache.TryGetValue(config.Id, out microserviceModel))
                {
                    Set(config.Id, config);
                }
            }
        }

        public void Set(int id, object config)
        {
            _cache.Set(id, config);
        }

        public async Task TryGetValue(int id, object conf, GetById getById)
        {
            if (!_cache.TryGetValue(id, out conf))
            {
                conf = _map.Map<ConfigModel>(await getById(id));
                if (conf == null)
                {
                    throw new EntityNotFoundException($"configuration {id} not found");
                }
            }
        }

        public void Remove(int id)
        {
            _cache.Remove(id);
        }
    }
}
