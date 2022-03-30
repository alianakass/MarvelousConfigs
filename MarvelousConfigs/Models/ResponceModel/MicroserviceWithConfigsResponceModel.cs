namespace MarvelousConfigs.API.Models
{
    public class MicroserviceWithConfigsResponceModel
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string URL { get; set; }
        public List<ConfigResponceModel> Configs { get; set; }

    }
}
