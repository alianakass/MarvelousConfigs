using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using Marvelous.Contracts.Urls;
using MarvelousConfigs.BLL.Helper.AuthRestClient;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth2;

namespace MarvelousConfigs.BLL.AuthRequestClient
{
    public class AuthRequestClient : IAuthRequestClient
    {
        private readonly ILogger<AuthRequestClient> _logger;
        private readonly string _url = "https://piter-education.ru:6042";

        public AuthRequestClient(ILogger<AuthRequestClient> logger)
        {
            _logger = logger;
        }

        public async Task<bool> CheckTokenForFront(string token)
        {
            _logger.LogInformation($"Start sending a request to validate a token for front");
            var client = new RestClient(new RestClientOptions(_url)
            {
                Timeout = 300000
            });
            client.Authenticator = new JwtAuthenticator(token.Split(" ")[1]); // new MarvelousAuthenticator(token);
            client.AddDefaultHeader(nameof(Microservice), Microservice.MarvelousConfigs.ToString());
            var request = new RestRequest($"{AuthUrls.ApiAuth}/{AuthUrls.ValidationFront}");
            _logger.LogInformation($"Getting a response from {Microservice.MarvelousAuth}");
            var response = await client.ExecuteAsync(request);
            return CheckTransactionError(response);
        }

        public async Task<string> GetToken(AuthRequestModel auth)
        {
            _logger.LogInformation($"Start sending a request to get token for admin auth");
            var client = new RestClient(new RestClientOptions(_url)
            {
                Timeout = 300000
            });
            client.AddDefaultHeader(nameof(Microservice), Microservice.MarvelousConfigs.ToString());
            var request = new RestRequest($"{AuthUrls.ApiAuth}/{AuthUrls.Login}", Method.Post);
            request.AddBody(auth);
            _logger.LogInformation($"Getting a response from {Microservice.MarvelousAuth}");
            var response = await client.ExecuteAsync(request);
            if (CheckTransactionError(response))
            {
                _logger.LogInformation($"Token has been received");
                return response.Content;
            }
            else
                throw new Exception($"Error occurred while getting the token for the admin with email {auth.Email} | {response.ErrorMessage}");
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
            client.Authenticator = new JwtAuthenticator(token.Split(" ")[1]); // new MarvelousAuthenticator(token); 
            client.AddDefaultHeader(nameof(Microservice), value: Microservice.MarvelousConfigs.ToString());
            var request = new RestRequest($"{AuthUrls.ApiAuth}/{AuthUrls.ValidationMicroservice}", Method.Get);
            _logger.LogInformation($"Getting a response from {Microservice.MarvelousAuth}");
            var response = await client.ExecuteAsync(request);
            return CheckTransactionError(response);
        }


        private bool CheckTransactionError(RestResponse response)
        {
            bool result = false;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogWarning(response.ErrorException.Message);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result = true;
            }
            if (response.Content == null)
            {
                _logger.LogWarning(response.ErrorException.Message);
            }
            return result;
        }
    }
}
