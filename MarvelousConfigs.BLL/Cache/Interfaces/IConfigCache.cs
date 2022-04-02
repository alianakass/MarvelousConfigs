using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.BLL.Services;

namespace MarvelousConfigs.BLL.Cache
{
    public interface IConfigCache
    {
        void Remove(int id);
        void Set(int id, object config);
        void SetCache(List<ConfigModel> configs);
        Task TryGetValue(int id, object conf, ConfigsService.GetById getById);
    }
}