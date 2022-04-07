using Dapper;
using MarvelousConfigs.DAL.Configuration;
using MarvelousConfigs.DAL.Entities;
using MarvelousConfigs.DAL.Helpers;
using Microsoft.Extensions.Options;
using System.Data;

namespace MarvelousConfigs.DAL.Repositories
{
    public class MicroservicesRepository : BaseRepository, IMicroserviceRepository
    {
        public MicroservicesRepository(IOptions<DbConfiguration> options) : base(options)
        {
        }

        public async Task<Microservice> GetMicroserviceById(int id)
        {
            using IDbConnection connection = ProvideConnection();

            return await connection.QueryFirstOrDefaultAsync<Microservice>
                (Queries.GetMicroserviceById, new { Id = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<Microservice>> GetAllMicroservices()
        {
            using IDbConnection connection = ProvideConnection();

            return (await connection.QueryAsync<Microservice>
                (Queries.GetAllMicroservices, commandType: CommandType.StoredProcedure)).ToList();
        }

        public async Task<int> AddMicroservice(Microservice microservice)
        {
            using IDbConnection connection = ProvideConnection();

            return await connection.QuerySingleAsync<int>
                (Queries.AddMicroservice, new { ServiceName = microservice.ServiceName, Url = microservice.Url, Address = microservice.Address },
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateMicroserviceById(int id, Microservice microservice)
        {
            using IDbConnection connection = ProvideConnection();

            var microservices = await connection.QueryAsync
                (Queries.UpdateMicroserviceById, new { Id = id, ServiceName = microservice.ServiceName, Url = microservice.Url, Address = microservice.Address },
                commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteOrRestoreMicroserviceById(int id, bool isDeleted)
        {
            using IDbConnection connection = ProvideConnection();

            await connection.QueryAsync
                (Queries.DeleteOrRestoreMicroserviceById, new { id, isDeleted }, commandType: CommandType.StoredProcedure);
        }

        public async Task<MicroserviceWithConfigs> GetMicroserviceWithConfigsById(int id)
        {
            using IDbConnection connection = ProvideConnection();

            Dictionary<int, MicroserviceWithConfigs> dict = new Dictionary<int, MicroserviceWithConfigs>();
            int serviceId = 0;

            await connection.QueryAsync<MicroserviceWithConfigs, Config, MicroserviceWithConfigs>
                (Queries.GetMicroserviceWithConfigsById, (service, conf) =>
                {
                    if (serviceId != service.Id)
                    {
                        dict.Add(service.Id, service);
                        serviceId = service.Id;
                        dict[serviceId].Configs = new List<Config>();
                    }

                    dict[serviceId].Configs.Add(conf);
                    return dict[serviceId];
                },
                new { Id = id }, splitOn: "Id", commandType: CommandType.StoredProcedure);

            return dict[serviceId];
        }
    }
}