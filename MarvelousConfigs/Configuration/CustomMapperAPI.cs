using AutoMapper;
using MarvelousConfigs.API.Models;
using MarvelousConfigs.BLL.Models;

namespace MarvelousConfigs.API.Configuration
{
    public class CustomMapperAPI : Profile
    {
        public CustomMapperAPI()
        {
            CreateMap<ConfigModel, ConfigResponceModel>();
            CreateMap<ConfigInputModel, ConfigModel>();

            CreateMap<MicroserviceModel, MicroserviceResponceModel>();
            CreateMap<MicroserviceInputModel, MicroserviceModel>();

            CreateMap<MicroserviceWithConfigsModel, MicroserviceWithConfigsResponceModel>()
                .ForMember(m => m.Configs, opt => opt.MapFrom(o => o.Configs));
        }
    }
}
