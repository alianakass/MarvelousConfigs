using AutoMapper;
using MarvelousConfigs.BLL.Exeptions;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.DAL.Entities;
using MarvelousConfigs.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace MarvelousConfigs.BLL.Services
{
    public class ConfigsService : IConfigsService
    {
        private readonly IConfigsRepository _rep;
        private readonly IMapper _map;
        private IMemoryCache _cache;

        public ConfigsService(IConfigsRepository repository, IMapper mapper, IMemoryCache cache)
        {
            _rep = repository;
            _map = mapper;
            _cache = cache;
            SetCache().Wait();
        }

        public async Task<int> AddConfig(ConfigModel config)
        {
            config.Created = DateTime.Now;
            int id = await _rep.AddConfig(_map.Map<Config>(config));

            if (id > 0)
            {
                await Set(id, config);
            }

            return id;
        }

        public async Task UpdateConfigById(int id, ConfigModel config)
        {
            ConfigModel conf = null;
            await TryGetValue(id, conf);

            config.Updated = DateTime.Now;
            await _rep.UpdateConfigById(id, _map.Map<Config>(config));

            _cache.Remove(id);
            await Set(id, _map.Map<ConfigModel>(_rep.GetConfigById(id)));
        }

        public async Task<List<ConfigModel>> GetAllConfigs()
        {
            return _map.Map<List<ConfigModel>>(await _rep.GetAllConfigs());
        }

        public async Task DeleteConfigById(int id)
        {
            ConfigModel conf = null;
            await TryGetValue(id, conf);
            DateTime updated = DateTime.Now;
            await _rep.DeleteOrRestoreConfigById(id, true, updated);
            _cache.Remove(id);
        }

        public async Task RestoreConfigById(int id)
        {
            ConfigModel conf = null;
            await TryGetValue(id, conf);
            DateTime updated = DateTime.Now;
            await _rep.DeleteOrRestoreConfigById(id, false, updated);
            await Set(id, conf);
        }

        private async Task SetCache()
        {
            var configs = _map.Map<List<ConfigModel>>(await _rep.GetAllConfigs());
            foreach (ConfigModel config in configs)
            {
                ConfigModel configModel = config;
                await TryGetValue(config.Id, config);
                await Set(config.Id, config);
            }
        }

        private async Task Set(int id, object config)
        {
            _cache.Set(id, config, new MemoryCacheEntryOptions().
                 SetSlidingExpiration(TimeSpan.FromHours(24)));
        }

        private async Task TryGetValue(int id, object conf)
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
    }
}
