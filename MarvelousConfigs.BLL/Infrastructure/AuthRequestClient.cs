using Auth.BusinessLayer.Helpers;
using FluentValidation;
using Marvelous.Contracts.Autentificator;
using Marvelous.Contracts.Endpoints;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using Marvelous.Contracts.ResponseModels;
using MarvelousConfigs.BLL.Infrastructure.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System.Net;

namespace MarvelousConfigs.BLL.Infrastructure
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
            _authUrl = _config[Microservice.MarvelousAuth.ToString()] + AuthEndpoints.ApiAuth;
        }

        public async Task<string> GetToken(AuthRequestModel auth)
        {
            _client.AddMicroservice(Microservice.MarvelousConfigs);
            var request = new RestRequest($"{_authUrl}{AuthEndpoints.Login}", Method.Post);
            request.AddBody(auth);
            var response = await _client.ExecuteAsync<string>(request);
            CheckTransactionError(response);
            if (string.IsNullOrEmpty(response.Content))
                throw new BadGatewayException($"Failed to convert responce data");
            return response.Content!;
        }

        public async Task<IdentityResponseModel> SendRequestToValidateToken(string jwtToken)
        {
            _client.Authenticator = new MarvelousAuthenticator(jwtToken);
            _client.AddMicroservice(Microservice.MarvelousConfigs);
            var request = new RestRequest($"{_authUrl}{AuthEndpoints.ValidationFront}");
            _logger.LogInformation($"Getting a response from {Microservice.MarvelousAuth}");
            var response = await _client.ExecuteAsync<IdentityResponseModel>(request);
            CheckTransactionError(response);
            if(response.Data is null)
                throw new BadGatewayException($"Failed to convert responce data");
            return response.Data!;
        }

        public async Task SendRequestWithToken(string token)
        {
            _client.Authenticator = new MarvelousAuthenticator(token);
            _client.AddMicroservice(Microservice.MarvelousConfigs);
            var request = new RestRequest($"{_authUrl}{AuthEndpoints.ValidationMicroservice}", Method.Get);
            _logger.LogInformation($"Getting a response from {Microservice.MarvelousAuth}");
            var response = await _client.ExecuteAsync<IdentityResponseModel>(request);
            CheckTransactionError(response);
        }

        private void CheckTransactionError(RestResponse response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedException($"Unauthorized | {response.ErrorException!.Message}");

                case HttpStatusCode.Forbidden:
                    throw new ForbiddenException($"Forbidden | {response.ErrorException!.Message}");

                case HttpStatusCode.BadRequest:
                    throw new BadRequestException($"Bad request | {response.ErrorException!.Message}");

                case HttpStatusCode.NotFound:
                    throw new EntityNotFoundException($"Not found | {response.ErrorException!.Message}");

                case HttpStatusCode.Conflict:
                    throw new ConflictException($"Conflict | {response.ErrorException!.Message}");

                case HttpStatusCode.UnprocessableEntity:
                    throw new ValidationException($"Unprocessable entity | {response.ErrorException!.Message}");

                case HttpStatusCode.ServiceUnavailable:
                    throw new ServiceUnavailableException($"Service unavailable | {response.ErrorException!.Message}");

                case HttpStatusCode.OK:
                    break;

                default:
                    throw new BadGatewayException($"{response.ErrorException!.Message}");
            }
        }
    }
}
