using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.BLL.Services;

namespace MarvelousConfigs.BLL.Cache
{
    public interface IMicroserviceCache
    {
        void Remove(int id);
        Task Set(int id, object service);
        Task SetCache(List<MicroserviceModel> microservices);
        Task TryGetValue(int id, object service, MicroservicesService.GetById getById);
    }
}