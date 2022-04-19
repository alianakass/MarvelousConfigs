
namespace MarvelousConfigs.BLL.Helper.Producer
{
    public interface IMarvelousConfigsProducer
    {
        Task NotifyConfigurationUpdated(int id);
    }
}