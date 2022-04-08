using Marvelous.Contracts.RequestModels;

namespace MarvelousConfigs.BLL.AuthRequestClient
{
    public interface IAuthRequestClient
    {
        Task<bool> CheckTokenForFront(string token);
        Task<bool> GetRestResponse(string token);
        Task<string> GetToken(AuthRequestModel auth);
    }
}