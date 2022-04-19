using Marvelous.Contracts.RequestModels;
using Marvelous.Contracts.ResponseModels;

namespace MarvelousConfigs.BLL.AuthRequestClient
{
    public interface IAuthRequestClient
    {
        Task<bool> SendRequestWithToken(string token);
        Task<string> GetToken(AuthRequestModel auth);
        Task<IdentityResponseModel> SendRequestToValidateToken(string jwtToken);
    }
}