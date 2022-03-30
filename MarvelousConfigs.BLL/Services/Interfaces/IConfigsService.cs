using MarvelousConfigs.BLL.Models;

namespace MarvelousConfigs.BLL.Services
{
    public interface IConfigsService
    {
        Task<int> AddConfig(ConfigModel config);
        Task DeleteConfigById(int id);
        Task<List<ConfigModel>> GetAllConfigs();
        Task RestoreConfigById(int id);
        Task UpdateConfigById(int id, ConfigModel config);
    }
}