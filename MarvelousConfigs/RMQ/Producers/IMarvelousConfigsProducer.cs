
namespace MarvelousConfigs.API.RMQ.Producers
{
    public interface IMarvelousConfigsProducer
    {
        Task NotifyConfigurationAdded(int id);
    }
}