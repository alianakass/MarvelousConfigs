using AutoMapper;
using MarvelousConfigs.BLL.Exeptions;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace MarvelousConfigs.BLL.Cache
{
    public class MicroserviceCache : IMicroserviceCache
    {

        private readonly IMemoryCache _cache;
        private readonly IMapper _map;
        private readonly IMicroserviceRepository _rep;

        public MicroserviceCache(IMemoryCache cache, IMapper mapper, IMicroserviceRepository repository)
        {
            _cache = cache;
            _map = mapper;
            _rep = repository;
        }

        public async Task SetCache()
        {
            var microservices = _map.Map<List<MicroserviceModel>>(await _rep.GetAllMicroservices());
            foreach (var microservice in microservices)
            {
                MicroserviceModel microserviceModel = microservice;
                if (!_cache.TryGetValue(microservice.Id, out microserviceModel))
                {
                    await Set(microservice.Id, microservice);
                }
            }
        }

        public async Task Set(int id, object service)
        {
            _cache.Set(id, service, new MemoryCacheEntryOptions().
                 SetSlidingExpiration(TimeSpan.FromHours(24)));
        }

        public async Task TryGetValue(int id, object service)
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

        public void Remove(int id)
        {
            _cache.Remove(id);
        }
    }
}
