﻿using AutoMapper;
using MarvelousConfigs.BLL.Exeptions;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microservice = MarvelousConfigs.DAL.Entities.Microservice;

namespace MarvelousConfigs.BLL.Services
{
    public class MicroservicesService : IMicroservicesService
    {
        private readonly IMicroserviceRepository _rep;
        private readonly IMemoryCache _cache;
        private readonly IMapper _map;

        public MicroservicesService(IMicroserviceRepository repository, IMapper mapper, IMemoryCache cache)
        {
            _rep = repository;
            _map = mapper;
            _cache = cache;
        }

        public async Task<int> AddMicroservice(MicroserviceModel microservice)
        {
            int id = await _rep.AddMicroservice(_map.Map<Microservice>(microservice));
            if (id > 0)
            {
                _cache.Set((Marvelous.Contracts.Enums.Microservice)id, microservice);
            }
            return id;
        }

        public async Task UpdateMicroservice(int id, MicroserviceModel microservice)
        {
            Microservice service = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                => _rep.GetMicroserviceById(id));

            if (service == null)
            {
                throw new EntityNotFoundException("");
            }

            await _rep.UpdateMicroserviceById(id, _map.Map<Microservice>(microservice));
            _cache.Set((Marvelous.Contracts.Enums.Microservice)id, _map.Map<MicroserviceModel>(await _rep.GetMicroserviceById(id)));
        }

        public async Task DeleteMicroservice(int id)
        {
            Microservice service = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                => _rep.GetMicroserviceById(id));

            if (service == null)
            {
                throw new EntityNotFoundException("");
            }

            await _rep.DeleteOrRestoreMicroserviceById(id, true);
            _cache.Remove(id);
        }

        public async Task RestoreMicroservice(int id)
        {
            Microservice service = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                 => _rep.GetMicroserviceById(id));

            if (service == null)
            {
                throw new EntityNotFoundException("");
            }

            await _rep.DeleteOrRestoreMicroserviceById(id, false);
            _cache.Set((Marvelous.Contracts.Enums.Microservice)id, service);
        }

        public async Task<List<MicroserviceModel>> GetAllMicroservices()
        {
            var services = _map.Map<List<MicroserviceModel>>(await _rep.GetAllMicroservices());
            return services;
        }

        public async Task<MicroserviceModel> GetMicroserviceById(int id)
        {
            Microservice service = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                => _rep.GetMicroserviceById(id));

            if (service == null)
            {
                throw new EntityNotFoundException("");
            }

            return _map.Map<MicroserviceModel>(service);
        }

        public async Task<MicroserviceWithConfigsModel> GetMicroserviceWithConfigsById(int id)
        {
            Microservice service = await _cache.GetOrCreateAsync(id, (ICacheEntry _)
                => _rep.GetMicroserviceById(id));

            if (service == null)
            {
                throw new EntityNotFoundException("");
            }

            var serviceWithConfigs = await _rep.GetMicroserviceWithConfigsById(id);
            return _map.Map<MicroserviceWithConfigsModel>(serviceWithConfigs);
        }
    }
}
