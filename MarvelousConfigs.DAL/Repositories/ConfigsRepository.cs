using Dapper;
using MarvelousConfigs.DAL.Configuration;
using MarvelousConfigs.DAL.Entities;
using MarvelousConfigs.DAL.Helpers;
using Microsoft.Extensions.Options;
using System.Data;

namespace MarvelousConfigs.DAL.Repositories
{
    public class ConfigsRepository : BaseRepository, IConfigsRepository
    {
        public ConfigsRepository(IOptions<DbConfiguration> options) : base(options)
        {
        }

        public async Task<Config> GetConfigById(int id)
        {
            using IDbConnection connection = ProvideConnection();

            return await connection.QueryFirstOrDefaultAsync<Config>
                (Queries.GetConfigById, new { Id = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<Config>> GetAllConfigs()
        {
            using IDbConnection connection = ProvideConnection();

            return (await connection.QueryAsync<Config>
                (Queries.GetAllConfigs, commandType: CommandType.StoredProcedure)).ToList();
        }

        public async Task<int> AddConfig(Config config)
        {
            using IDbConnection connection = ProvideConnection();

            return await connection.QuerySingleAsync<int>
                (Queries.AddConfig, new { Key = config.Key, Value = config.Value, ServiceId = config.ServiceId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateConfigById(int id, Config config)
        {
            using IDbConnection connection = ProvideConnection();

            await connection.QueryAsync
                (Queries.UpdateConfigById,
                new { Id = id, Key = config.Key, Value = config.Value, ServiceId = config.ServiceId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteOrRestoreConfigById(int id, bool isDeleted)
        {
            using IDbConnection connection = ProvideConnection();

            await connection.QueryAsync
                (Queries.DeleteOrRestoreConfigById, new { Id = id, IsDeleted = isDeleted }, commandType: CommandType.StoredProcedure);
        }
    }
}
