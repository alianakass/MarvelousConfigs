using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Authenticators;

namespace MarvelousConfigs.BLL.AuthRequestClient
{
    public class AuthRequestClient : IAuthRequestClient
    {
        private readonly ILogger<AuthRequestClient> _logger;
        private readonly string _path = "check-validate-token-microservices";
        private readonly string _url = "https://80.78.240.16:7086";

        public AuthRequestClient(ILogger<AuthRequestClient> logger)
        {
            _logger = logger;
        }

        public async Task<bool> GetRestResponse(string token)
        {
            _logger.LogInformation($"Start sending a request to validate a token");
            bool response = false;
            try
            {
                response = await SendRequestWithToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while validating token");
            }

            return response;
        }

        private async Task<bool> SendRequestWithToken(string token)
        {
            var client = new RestClient(new RestClientOptions(_url)
            {
                Timeout = 300000
            });
            client.Authenticator = new JwtAuthenticator(token);
            var request = new RestRequest(_path, Method.Get);
            _logger.LogInformation($"Getting a response from {Microservice.MarvelousAuth}");
            var response = await client.ExecuteAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else
                return false;
        }
    }
}
