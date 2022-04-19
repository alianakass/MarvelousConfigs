using AutoMapper;
using MarvelousConfigs.BLL.AuthRequestClient;
using MarvelousConfigs.BLL.Cache;
using MarvelousConfigs.BLL.Configuration;
using MarvelousConfigs.BLL.Exeptions;
using MarvelousConfigs.BLL.Helper.Producer;
using MarvelousConfigs.BLL.Models;
using MarvelousConfigs.BLL.Services;
using MarvelousConfigs.BLL.Tests;
using MarvelousConfigs.DAL.Entities;
using MarvelousConfigs.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarvelousConfig.BLL.Tests
{
    public class ConfigsServiceTests : BaseTest<ConfigsService>
    {
        private Mock<IConfigsRepository> _repositoryMock;
        private IMapper _map;
        private IConfigsService _service;
        private IMemoryCache _cache;
        private Mock<IAuthRequestClient> _auth;
        private Mock<IMemoryCacheExtentions> _memory;
        private Mock<IMarvelousConfigsProducer> _producer;


        [SetUp]
        public void Setup()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _repositoryMock = new Mock<IConfigsRepository>();
            _logger = new Mock<ILogger<ConfigsService>>();
            _auth = new Mock<IAuthRequestClient>();
            _memory = new Mock<IMemoryCacheExtentions>();
            _producer = new Mock<IMarvelousConfigsProducer>();
            _map = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CustomMapperBLL>()));
            _service = new ConfigsService(_repositoryMock.Object, _map, _cache, _memory.Object,
                _logger.Object, _auth.Object, _producer.Object);

        }

        [Test]
        public async Task GetAllConfigsTest()
        {
            //given
            _repositoryMock.Setup(x => x.GetAllConfigs()).ReturnsAsync(It.IsAny<List<Config>>());

            //when
            List<ConfigModel>? actual = await _service.GetAllConfigs();

            //then
            List<ConfigModel>? actual = await _service.GetAllConfigs();

            //when
            _repositoryMock.Verify(x => x.GetAllConfigs(), Times.Once);
        }

        [TestCaseSource(typeof(UpdateConfigByIdTestCaseSource))]
        public async Task UpdateConfigByIdTest(int id, Config config, ConfigModel model)
        {
            //given
            _repositoryMock.Setup(x => x.GetConfigById(id)).ReturnsAsync(config);
            _repositoryMock.Setup(x => x.UpdateConfigById(id, config));

            //when
            await _service.UpdateConfigById(id, model);

            //then
            _repositoryMock.Verify(x => x.GetConfigById(id));
            _repositoryMock.Verify(x => x.UpdateConfigById(id, It.IsAny<Config>()), Times.Once);
        }

        [TestCase(3)]
        public void UpdateConfigById_WhenConfigNotFound_ShouldThrowEntityNotFoundException(int id)
        {
            //given
            _repositoryMock.Setup(x => x.UpdateConfigById(id, It.IsAny<Config>()));

            //when

            //then
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _service.UpdateConfigById(id, It.IsAny<ConfigModel>()));
            _repositoryMock.Verify(x => x.GetConfigById(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateConfigById(It.IsAny<int>(), It.IsAny<Config>()), Times.Never);
        }
    }
}