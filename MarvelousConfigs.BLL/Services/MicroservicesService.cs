using AutoMapper;
using MarvelousConfigs.BLL.Exeptions;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.DAL.Entities;
using MarvelousConfigs.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace MarvelousConfigs.BLL.Services
{
    public class MicroservicesService : IMicroservicesService
    {
        private readonly IMicroserviceRepository _rep;
        private readonly IMapper _map;
        private IMemoryCache _cache;

        public MicroservicesService(IMicroserviceRepository repository, IMapper mapper, IMemoryCache cache)
        {
            _rep = repository;
            _map = mapper;
            _cache = cache;
            SetCache().Wait();
        }

        public async Task<int> AddMicroservice(MicroserviceModel microservice)
        {
            int id = await _rep.AddMicroservice(_map.Map<Microservice>(microservice));
            if (id > 0)
            {
                await Set(id, microservice);
            }
            return id;
        }

        public async Task UpdateMicroservice(int id, MicroserviceModel microservice)
        {
            MicroserviceModel service = null;
            await TryGetValue(id, service);
            await _rep.UpdateMicroserviceById(id, _map.Map<Microservice>(microservice));
            _cache.Remove(id);
            await Set(id, _map.Map<MicroserviceModel>(await _rep.GetMicroserviceById(id)));
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
            await TryGetValue(id, service);
            var serviceWithConfigs = await _rep.GetMicroserviceWithConfigsById(id);
            return _map.Map<MicroserviceWithConfigsModel>(serviceWithConfigs);
        }

        public async Task DeleteMicroservice(int id)
        {
            MicroserviceModel service = null;
            await TryGetValue(id, service);
            await _rep.DeleteOrRestoreMicroserviceById(id, true);
            _cache.Remove(id);
        }

        public async Task RestoreMicroservice(int id)
        {
            MicroserviceModel service = null;
            await TryGetValue(id, service);
            await _rep.DeleteOrRestoreMicroserviceById(id, false);
            await Set(id, service);
        }

        private async Task SetCache()
        {
            var microservices = _map.Map<List<MicroserviceModel>>(await GetAllMicroservices());
            foreach (var microservice in microservices)
            {
                MicroserviceModel microserviceModel = microservice;
                if (!_cache.TryGetValue(microservice.Id, out microserviceModel))
                {
                    await Set(microservice.Id, microservice);
                }
            }
        }

        private async Task Set(int id, object service)
        {
            _cache.Set(id, service, new MemoryCacheEntryOptions().
                 SetSlidingExpiration(TimeSpan.FromHours(24)));
        }

        private async Task TryGetValue(int id, object service)
        {
            if (!_cache.TryGetValue(id, out service))
            {
                service = _map.Map<MicroserviceModel>(await _rep.GetMicroserviceById(id));
                if (service == null)
                {
                    throw new EntityNotFoundException($"microservice {id} not found");
                }
            }
        }
    }
}
