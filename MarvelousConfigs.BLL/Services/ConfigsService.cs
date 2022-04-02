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
        private readonly IConfigCache _cache;
        public delegate Task<Config> GetById(int id);
        public GetById getById;

        public ConfigsService(IConfigsRepository repository, IMapper mapper, IConfigCache cache)
        {
            _rep = repository;
            _map = mapper;
            _cache = cache;
            getById = new GetById(_rep.GetConfigById);
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

        public async Task SetCache()
        {
            List<ConfigModel> conf = _map.Map<List<ConfigModel>>(await _rep.GetAllConfigs());
            _cache.SetCache(conf);
        }

        public async Task UpdateConfigById(int id, ConfigModel config)
        {
            ConfigModel conf = null;
            await _cache.TryGetValue(id, conf, getById);

            config.Updated = DateTime.Now;
            await _rep.UpdateConfigById(id, _map.Map<Config>(config));

            _cache.Set(id, _map.Map<ConfigModel>(((_rep.GetConfigById(id).Result))));
        }

        public async Task DeleteConfigById(int id)
        {
            ConfigModel conf = _map.Map<ConfigModel>(getById);
            await _cache.TryGetValue(id, conf, getById);
            DateTime updated = DateTime.Now;
            await _rep.DeleteOrRestoreConfigById(id, true, updated);
            _cache.Remove(id);
        }

        public async Task RestoreConfigById(int id)
        {
            ConfigModel conf = _map.Map<ConfigModel>(getById);
            await _cache.TryGetValue(id, conf, getById);
            DateTime updated = DateTime.Now;
            await _rep.DeleteOrRestoreConfigById(id, false, updated);
            _cache.Set(id, conf);
        }

        public async Task<List<ConfigModel>> GetAllConfigs()
        {
            var cfg = _map.Map<List<ConfigModel>>(await _rep.GetAllConfigs());
            _cache.SetCache(cfg);
            return cfg;
        }
    }
}
