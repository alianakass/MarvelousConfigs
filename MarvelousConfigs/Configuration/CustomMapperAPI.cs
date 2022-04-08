using AutoMapper;
using Marvelous.Contracts.ExchangeModels;
using MarvelousConfigs.API.Models;
using MarvelousConfigs.BLL.Models;

namespace MarvelousConfigs.API.Configuration
{
    public class CustomMapperAPI : Profile
    {
        public CustomMapperAPI()
        {
            CreateMap<ConfigInputModel, ConfigModel>();
            CreateMap<ConfigModel, ConfigOutputModel>();
            CreateMap<ConfigModel, ConfigExchangeModel>();

            CreateMap<MicroserviceInputModel, MicroserviceModel>();
            CreateMap<MicroserviceModel, MicroserviceOutputModel>();

            CreateMap<MicroserviceWithConfigsModel, MicroserviceWithConfigsOutputModel>()
                .ForMember(m => m.Configs, opt => opt.MapFrom(o => o.Configs));
        }
    }
}
