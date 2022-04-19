using MarvelousConfigs.BLL.Infrastructure;
using MarvelousConfigs.BLL.Infrastructure.Exceptions;
using MarvelousConfigs.DAL.Entities;
using MarvelousConfigs.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarvelousConfigs.BLL.Tests
{
    internal class MemoryCacheExtentionsTests : BaseVerifyTest<MemoryCacheExtentions>
    {
        private IMemoryCache _cache;
        private Mock<IMarvelousConfigsProducer> _prod;
        private Mock<IConfigsRepository> _config;
        private Mock<IMicroserviceRepository> _microservice;
        private IMemoryCacheExtentions _extentions;

        [SetUp]
        public void Setup()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _microservice = new Mock<IMicroserviceRepository>();
            _config = new Mock<IConfigsRepository>();
            _logger = new Mock<ILogger<MemoryCacheExtentions>>();
            _prod = new Mock<IMarvelousConfigsProducer>();
            _extentions = new MemoryCacheExtentions(_cache, _microservice.Object, _config.Object, _logger.Object, _prod.Object);
        }

        [Test]
        public async Task SetMemoryCacheTest()
        {
            List<Config> configs = new List<Config>() {
            new Config()
            {
                Id = 1,
                Key = "KEY",
                Value = "VALUE",
                ServiceId = 1
            },
            new Config()
            {
                Id = 2,
                Key = "KEY",
                Value = "VALUE",
                ServiceId = 1
            },
            new Config()
            {
                Id = 3,
                Key = "KEY",
                Value = "VALUE",
                ServiceId = 2
            }};

            List<Microservice> services = new List<Microservice>() {
            new Microservice()
            {
                Id = 1,
                ServiceName = "Name1",
                Url = "URL1"
            },
            new Microservice()
            {
                Id = 2,
                ServiceName = "Name2",
                Url = "URL2"
            },
            new Microservice()
            {
                Id = 3,
                ServiceName = "Name3",
                Url = "URL3"
            }};

            _microservice.Setup(x => x.GetAllMicroservices()).ReturnsAsync(services);
            _config.Setup(x => x.GetAllConfigs()).ReturnsAsync(configs);

            await _extentions.SetMemoryCache();

            _microservice.Verify(x => x.GetAllMicroservices(), Times.Once);
            _config.Verify(x => x.GetAllConfigs(), Times.Once);
            VerifyLogger(LogLevel.Information, 2);

            foreach (var s in services)
            {
                List<Config> actualCfg = (List<Config>)_cache.Get(s.ServiceName);
                foreach (var a in actualCfg)
                {
                    Assert.AreEqual(a.ServiceId, s.Id);
                }
            }
        }

        [Test]
        public async Task SetMemoryCacheTest_WhenExceptionOccurredWhileCacheLoading_ShouldThrowCacheLoadingException()
        {
            List<Config> configs = new List<Config>() {
            new Config()
            {
                Id = 1,
                Key = "KEY",
                Value = "VALUE",
                ServiceId = 1
            },
            new Config()
            {
                Id = 2,
                Key = "KEY",
                Value = "VALUE",
            },
            new Config()
            {
                Key = "KEY",
                Value = "VALUE",
                ServiceId = 2
            }};

            List<Microservice> services = new List<Microservice>() {
            new Microservice()
            {
                Id = 14567890,
                Url = "URL1"
            },
            new Microservice()
            {
                ServiceName = "Name2",
                Url = "URL2"
            },
            new Microservice()
            {
                Id = 3,
                ServiceName = "Name3",
            }};

            _microservice.Setup(x => x.GetAllMicroservices()).ReturnsAsync(services);
            _config.Setup(x => x.GetAllConfigs()).ReturnsAsync(configs);
            _prod.Setup(x => x.NotifyAdminAboutErrorToEmail(It.IsAny<string>()));

            Assert.ThrowsAsync<CacheLoadingException>(async () => await _extentions.SetMemoryCache());
            _microservice.Verify(x => x.GetAllMicroservices(), Times.Once);
            _config.Verify(x => x.GetAllConfigs(), Times.Once);
            _prod.Verify(x => x.NotifyAdminAboutErrorToEmail(It.IsAny<string>()), Times.Once);
            VerifyLogger(LogLevel.Information, 1);

        }

        [TestCase(3)]
        public async Task RefreshConfigByServiceIdTest(int id)
        {
            List<Config> configs = new List<Config>() {
            new Config()
            {
                Id = 1,
                Key = "KEY",
                Value = "VALUE",
                ServiceId = 3
            },
            new Config()
            {
                Id = 2,
                Key = "KEY",
                Value = "VALUE",
                ServiceId = 3
            },
            new Config()
            {
                Id = 3,
                Key = "KEY",
                Value = "VALUE",
                ServiceId = 3
            }};

            Microservice service = new Microservice()
            {
                Id = 3,
                ServiceName = "Name1",
                Url = "URL1"
            };

            _microservice.Setup(x => x.GetMicroserviceById(id)).ReturnsAsync(service);
            _config.Setup(x => x.GetConfigsByService(service.ServiceName)).ReturnsAsync(configs);

            await _extentions.RefreshConfigByServiceId(id);

            _microservice.Verify(x => x.GetMicroserviceById(id), Times.Once);
            _config.Verify(x => x.GetConfigsByService(service.ServiceName), Times.Once);
            VerifyLogger(LogLevel.Information, 2);

            List<Config> actualCfg = (List<Config>)_cache.Get(service.ServiceName);
            foreach (var a in actualCfg)
            {
                Assert.AreEqual(a.ServiceId, service.Id);
            }
        }

        [TestCase(3)]
        public async Task RefreshConfigByServiceIdTest_WhenExceptionOccurredWhileCacheLoading_ShouldThrowCacheLoadingException(int id)
        {
            List<Config> configs = new List<Config>() {
            new Config()
            {
                Id = 1,
                Key = "KEY",
                Value = "VALUE",
                ServiceId = 1
            },
            new Config()
            {
                Id = 2,
                Key = "KEY",
                Value = "VALUE",
            },
            new Config()
            {
                Key = "KEY",
                Value = "VALUE",
                ServiceId = 2
            }};

            Microservice service = new Microservice()
            {
                Id = 3,
                Url = "URL1"
            };

            _microservice.Setup(x => x.GetMicroserviceById(id)).ReturnsAsync(service);
            _config.Setup(x => x.GetConfigsByService(service.ServiceName)).ReturnsAsync(configs);
            _prod.Setup(x => x.NotifyAdminAboutErrorToEmail(It.IsAny<string>()));

            Assert.ThrowsAsync<CacheLoadingException>(async () => await _extentions.RefreshConfigByServiceId(id));
            _microservice.Verify(x => x.GetMicroserviceById(id), Times.Once);
            _config.Verify(x => x.GetConfigsByService(service.ServiceName), Times.Once);
            _prod.Verify(x => x.NotifyAdminAboutErrorToEmail(It.IsAny<string>()), Times.Once);
            VerifyLogger(LogLevel.Information, 1);

        }
    }
}
