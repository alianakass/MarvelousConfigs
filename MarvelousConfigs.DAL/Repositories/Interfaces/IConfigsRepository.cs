using MarvelousConfigs.DAL.Entities;

namespace MarvelousConfigs.DAL.Repositories
{
    public interface IConfigsRepository
    {
        Task<int> AddConfig(Config config);
        Task DeleteOrRestoreConfigById(int id, bool IsDeleted, DateTime date);
        Task<List<Config>> GetAllConfigs();
        Task<Config> GetConfigById(int id);
        Task UpdateConfigById(int id, Config config);
    }
}