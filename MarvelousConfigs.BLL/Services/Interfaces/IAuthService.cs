
namespace MarvelousConfigs.BLL.Services
{
    public interface IAuthService
    {
        Task<string> GetToken(string email, string pass);
    }
}