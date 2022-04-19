using Auth.BusinessLayer.Helpers;
using Marvelous.Contracts.Autentificator;
using Marvelous.Contracts.Endpoints;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using Marvelous.Contracts.ResponseModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace MarvelousConfigs.BLL.AuthRequestClient
{
    public class AuthRequestClient : IAuthRequestClient
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AuthRequestClient> _logger;
        private readonly IRestClient _client;
        private readonly string _authUrl;

        public AuthRequestClient(ILogger<AuthRequestClient> logger, IConfiguration configuration, IRestClient client)
        {
            _logger = logger;
            _config = configuration;
            _client = client;
            _authUrl = $"{_config[Microservice.MarvelousAuth.ToString()]}{AuthEndpoints.ApiAuth}";
        }

        public async Task<string> GetToken(AuthRequestModel auth)
        {
            _client.AddMicroservice(Microservice.MarvelousConfigs);
            var request = new RestRequest($"{_authUrl}{AuthEndpoints.Login}", Method.Post);
            request.AddBody(auth);
            var response = await _client.ExecuteAsync<string>(request);
            CheckTransactionError(response);
            return response.Content;
        }

        public async Task<IdentityResponseModel> SendRequestToValidateToken(string jwtToken)
        {
            var request = new RestRequest($"{_authUrl}{AuthEndpoints.ValidationFront}");
            _client.Authenticator = new MarvelousAuthenticator(jwtToken);
            _client.AddMicroservice(Microservice.MarvelousConfigs);
            var response = await _client.ExecuteAsync<IdentityResponseModel>(request);
            CheckTransactionError(response);
            return response.Data;
        }

        public async Task<bool> SendRequestWithToken(string token)
        {
            _client.Authenticator = new MarvelousAuthenticator(token);
            _client.AddMicroservice(Microservice.MarvelousConfigs);
            var request = new RestRequest($"{_authUrl}{AuthEndpoints.ValidationMicroservice}", Method.Get);
            _logger.LogInformation($"Getting a response from {Microservice.MarvelousAuth}");
            var response = await _client.ExecuteAsync<IdentityResponseModel>(request);
            return CheckTransactionError(response);
        }

        private bool CheckTransactionError(RestResponse response)
        {
            bool result = false;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogWarning(response.ErrorException!.Message);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result = true;
            }
            if (response.Content == null)
            {
                _logger.LogWarning(response.ErrorException!.Message);
            }
            return result;
        }
    }
}
