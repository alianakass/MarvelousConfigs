
namespace MarvelousConfigs.BLL.Cache
{
    public interface IConfigCache
    {
        void Remove(int id);
        void Set(int id, object config);
        Task SetCache();
        Task TryGetValue(int id, object conf);
    }
}