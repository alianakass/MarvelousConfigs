using AutoMapper;
using MarvelousConfigs.BLL.Cache;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.DAL.Entities;
using MarvelousConfigs.DAL.Repositories;

namespace MarvelousConfigs.BLL.Services
{
    public class ConfigsService : IConfigsService
    {
        private readonly IConfigsRepository _rep;
        private readonly IMapper _map;
        private IConfigCache _cache;

        public ConfigsService(IConfigsRepository repository, IMapper mapper, IConfigCache cache)
        {
            _rep = repository;
            _map = mapper;
            _cache = cache;
        }

        public async Task<int> AddConfig(ConfigModel config)
        {
            config.Created = DateTime.Now;
            int id = await _rep.AddConfig(_map.Map<Config>(config));

            if (id > 0)
            {
                _cache.Set(id, config);
            }

            return id;
        }

        public async Task UpdateConfigById(int id, ConfigModel config)
        {
            ConfigModel conf = null;
            await _cache.TryGetValue(id, conf);

            config.Updated = DateTime.Now;
            await _rep.UpdateConfigById(id, _map.Map<Config>(config));

            //_cache.Remove(id);
            _cache.Set(id, _map.Map<ConfigModel>(_rep.GetConfigById(id)));
        }

        public async Task<List<ConfigModel>> GetAllConfigs()
        {
            return _map.Map<List<ConfigModel>>(await _rep.GetAllConfigs());
        }

        public async Task DeleteConfigById(int id)
        {
            ConfigModel conf = null;
            await _cache.TryGetValue(id, conf);
            DateTime updated = DateTime.Now;
            await _rep.DeleteOrRestoreConfigById(id, true, updated);
            _cache.Remove(id);
        }

        public async Task RestoreConfigById(int id)
        {
            ConfigModel conf = null;
            await _cache.TryGetValue(id, conf);
            DateTime updated = DateTime.Now;
            await _rep.DeleteOrRestoreConfigById(id, false, updated);
            _cache.Set(id, conf);
        }

    }
}
