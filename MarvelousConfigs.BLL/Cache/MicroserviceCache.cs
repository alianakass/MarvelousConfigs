using AutoMapper;
using MarvelousConfigs.BLL.Exeptions;
using MarvelousConfigs.BLL.Models;
using Microsoft.Extensions.Caching.Memory;
using static MarvelousConfigs.BLL.Services.MicroservicesService;

namespace MarvelousConfigs.BLL.Cache
{
    public class MicroserviceCache : IMicroserviceCache
    {

        private readonly IMemoryCache _cache;
        private readonly IMapper _map;

        public MicroserviceCache(IMemoryCache cache, IMapper mapper)
        {
            _cache = cache;
            _map = mapper;
        }

        public async Task SetCache(List<MicroserviceModel> microservices)
        {
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
            _cache.Set(id, service);
        }

        public async Task TryGetValue(int id, object service, GetById getById)
        {
            if (!_cache.TryGetValue(id, out service))
            {
                service = _map.Map<MicroserviceModel>(await getById(id));
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
