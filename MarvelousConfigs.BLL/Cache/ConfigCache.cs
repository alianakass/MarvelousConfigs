using AutoMapper;
using MarvelousConfigs.BLL.Exeptions;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace MarvelousConfigs.BLL.Cache
{
    public class ConfigCache : IConfigCache
    {
        private readonly IMemoryCache _cache;
        private readonly IMapper _map;
        private readonly IConfigsRepository _rep;
        public ConfigCache(IMemoryCache cache, IMapper mapper, IConfigsRepository repository)
        {
            _cache = cache;
            _map = mapper;
            _rep = repository;
        }

        public async Task SetCache()
        {
            var configs = _map.Map<List<ConfigModel>>(await _rep.GetAllConfigs());
            foreach (ConfigModel config in configs)
            {
                ConfigModel configModel = config;
                await TryGetValue(config.Id, config);
                Set(config.Id, config);
            }
        }

        public void Set(int id, object config)
        {
            var options = new MemoryCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromHours(12),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
            };
            _cache.Set(id, config, options);
        }

        public async Task TryGetValue(int id, object conf)
        {
            if (!_cache.TryGetValue(id, out conf))
            {
                conf = _map.Map<ConfigModel>(await _rep.GetConfigById(id));
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
