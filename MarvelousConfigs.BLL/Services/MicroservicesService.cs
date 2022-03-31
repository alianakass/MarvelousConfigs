using AutoMapper;
using MarvelousConfigs.BLL.Cache;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.DAL.Entities;
using MarvelousConfigs.DAL.Repositories;

namespace MarvelousConfigs.BLL.Services
{
    public class MicroservicesService : IMicroservicesService
    {
        private readonly IMicroserviceRepository _rep;
        private readonly IMapper _map;
        private IMicroserviceCache _cache;

        public MicroservicesService(IMicroserviceRepository repository, IMapper mapper, IMicroserviceCache cache)
        {
            _rep = repository;
            _map = mapper;
            _cache = cache;
        }

        public async Task<int> AddMicroservice(MicroserviceModel microservice)
        {
            int id = await _rep.AddMicroservice(_map.Map<Microservice>(microservice));
            if (id > 0)
            {
                await _cache.Set(id, microservice);
            }
            return id;
        }

        public async Task UpdateMicroservice(int id, MicroserviceModel microservice)
        {
            MicroserviceModel service = null;
            await _cache.TryGetValue(id, service);
            await _rep.UpdateMicroserviceById(id, _map.Map<Microservice>(microservice));
            //_cache.Remove(id);
            await _cache.Set(id, _map.Map<MicroserviceModel>(await _rep.GetMicroserviceById(id)));
        }

        public async Task<List<MicroserviceModel>> GetAllMicroservices()
        {
            return _map.Map<List<MicroserviceModel>>(await _rep.GetAllMicroservices());
        }

        public async Task<List<MicroserviceWithConfigsModel>> GetAllMicroservicesWithConfigs()
        {
            return _map.Map<List<MicroserviceWithConfigsModel>>(await _rep.GetAllMicroservicesWithConfigs());
        }

        public async Task<MicroserviceWithConfigsModel> GetMicroserviceWithConfigsById(int id)
        {
            MicroserviceModel service = null;
            await _cache.TryGetValue(id, service);
            var serviceWithConfigs = await _rep.GetMicroserviceWithConfigsById(id);
            return _map.Map<MicroserviceWithConfigsModel>(serviceWithConfigs);
        }

        public async Task DeleteMicroservice(int id)
        {
            MicroserviceModel service = null;
            await _cache.TryGetValue(id, service);
            await _rep.DeleteOrRestoreMicroserviceById(id, true);
            _cache.Remove(id);
        }

        public async Task RestoreMicroservice(int id)
        {
            MicroserviceModel service = null;
            await _cache.TryGetValue(id, service);
            await _rep.DeleteOrRestoreMicroserviceById(id, false);
            await _cache.Set(id, service);
        }
    }
}
