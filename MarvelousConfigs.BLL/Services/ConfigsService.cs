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
        private readonly IMemoryCache _cache;
        public delegate Task<Config> GetById(int id);
        public GetById getById;

        public ConfigsService(IConfigsRepository repository, IMapper mapper, IMemoryCache cache)
        {
            _rep = repository;
            _map = mapper;
            _cache = cache;
            getById = new GetById(_rep.GetConfigById);
        }

        public async Task<int> AddConfig(ConfigModel config)
        {
            int id = await _rep.AddConfig(_map.Map<Config>(config));

            if (id > 0)
            {
                _cache.Set(id, config);
            }

            return id;
        }

        public async Task UpdateConfigById(int id, ConfigModel config)
        {
            Config conf = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                => _rep.GetConfigById(id));

            if (conf == null)
            {
                throw new EntityNotFoundException("");
            }

            await _rep.UpdateConfigById(id, _map.Map<Config>(config));

            _cache.Set(id, _map.Map<ConfigModel>(((_rep.GetConfigById(id).Result))));
        }

        public async Task DeleteConfigById(int id)
        {

            Config conf = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                => _rep.GetConfigById(id));

            if (conf == null)
            {
                throw new EntityNotFoundException("");
            }

            await _rep.DeleteOrRestoreConfigById(id, true);
            _cache.Remove(id);
        }

        public async Task RestoreConfigById(int id)
        {
            Config conf = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                => _rep.GetConfigById(id));

            if (conf == null)
            {
                throw new EntityNotFoundException("");
            }

            await _rep.DeleteOrRestoreConfigById(id, false);
            _cache.Set(id, conf);
        }

        public async Task<List<ConfigModel>> GetAllConfigs()
        {
            var cfg = _map.Map<List<ConfigModel>>(await _rep.GetAllConfigs());
            return cfg;
        }

        public async Task<List<ConfigModel>> GetConfigsByServiceId(int id)
        {
            var configs = _map.Map<List<ConfigModel>>(await _rep.GetConfigsByServiceId(id));
            return configs;
        }
    }
}
