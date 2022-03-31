
namespace MarvelousConfigs.BLL.Cache
{
    public interface IMicroserviceCache
    {
        void Remove(int id);
        Task Set(int id, object service);
        Task SetCache();
        Task TryGetValue(int id, object service);
    }
}