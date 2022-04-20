using Auth.BusinessLayer.Helpers;
using FluentValidation;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using Marvelous.Contracts.ResponseModels;
using MarvelousConfigs.BLL.Infrastructure;
using MarvelousConfigs.BLL.Infrastructure.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MarvelousConfigs.BLL.Tests
{
    internal class AuthRestClientTests : BaseVerifyTest<AuthRequestClient>
    {
        private IConfiguration _config;
        private Mock<IRestClient> _client;
        private IAuthRequestClient _authRequestClient;
        private const string _mess = "test exception message";

        [SetUp]
        public void SetUp()
        {
            _client = new Mock<IRestClient>();
            _logger = new Mock<ILogger<AuthRequestClient>>();
            _config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>()).Build();
            _config[Microservice.MarvelousAuth.ToString()] = "https://piter-education.ru:6042";
            _authRequestClient = new AuthRequestClient(_logger.Object, _config, _client.Object);
        }

        #region get token test

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public async Task GetTokenTest_WhenAuthServiceReturnedHttpCode401Unauthorized_ShouldThrowUnauthorizedException(AuthRequestModel auth)
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<string>>(x => x.Content == token && x.StatusCode ==
            HttpStatusCode.Unauthorized && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<UnauthorizedException>(async () => await _authRequestClient.GetToken(auth));
            VerifyRequestTests<string>(_client);
        }

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public async Task GetTokenTest_WhenAuthServiceReturnedHttpCode403Forbidden_ShouldThrowForbiddenException(AuthRequestModel auth)
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<string>>(x => x.Content == token && x.StatusCode ==
            HttpStatusCode.Forbidden && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<ForbiddenException>(async () => await _authRequestClient.GetToken(auth));
            VerifyRequestTests<string>(_client);
        }

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public async Task GetTokenTest_WhenAuthServiceReturnedHttpCode400BadRequest_ShouldThrowBadRequestException(AuthRequestModel auth)
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<string>>(x => x.Content == token && x.StatusCode ==
            HttpStatusCode.BadRequest && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<BadRequestException>(async () => await _authRequestClient.GetToken(auth));
            VerifyRequestTests<string>(_client);
        }

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public async Task GetTokenTest_WhenAuthServiceReturnedHttpCode404NotFound_ShouldThrowEntityNotFoundException(AuthRequestModel auth)
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<string>>(x => x.Content == token && x.StatusCode ==
            HttpStatusCode.NotFound && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _authRequestClient.GetToken(auth));
            VerifyRequestTests<string>(_client);
        }

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public async Task GetTokenTest_WhenAuthServiceReturnedHttpCode409Conflict_ShouldThrowConflictException(AuthRequestModel auth)
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<string>>(x => x.Content == token && x.StatusCode ==
            HttpStatusCode.Conflict && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<ConflictException>(async () => await _authRequestClient.GetToken(auth));
            VerifyRequestTests<string>(_client);
        }

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public async Task GetTokenTest_WhenAuthServiceReturnedHttpCode422UnprocessableEntity_ShouldThrowValidationException(AuthRequestModel auth)
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<string>>(x => x.Content == token && x.StatusCode ==
            HttpStatusCode.UnprocessableEntity && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<ValidationException>(async () => await _authRequestClient.GetToken(auth));
            VerifyRequestTests<string>(_client);
        }

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public async Task GetTokenTest_WhenAuthServiceReturnedHttpCode503ServiceUnavailable_ShouldThrowServiceUnavailableException(AuthRequestModel auth)
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<string>>(x => x.Content == token && x.StatusCode ==
            HttpStatusCode.ServiceUnavailable && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<ServiceUnavailableException>(async () => await _authRequestClient.GetToken(auth));
            VerifyRequestTests<string>(_client);
        }

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public async Task GetTokenTest_WhenAuthServiceReturnedNotProvidedHttpCode_ShouldThrowServiceBadGatewayException(AuthRequestModel auth)
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<string>>(x => x.Content == token && x.StatusCode ==
            HttpStatusCode.Unused && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<BadGatewayException>(async () => await _authRequestClient.GetToken(auth));
            VerifyRequestTests<string>(_client);
        }

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public async Task GetTokenTest_WhenAuthServiceAnswerIsEmpty_ShouldThrowServiceBadGatewayException(AuthRequestModel auth)
        {
            var responce = Mock.Of<RestResponse<string>>(x => x.StatusCode == HttpStatusCode.OK);
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<BadGatewayException>(async () => await _authRequestClient.GetToken(auth));
            VerifyRequestTests<string>(_client);
        }

        [TestCaseSource(typeof(GetTokenTestCaseSource))]
        public async Task GetTokenTest_WhenAuthServiceReturnedHttpCode200Ok(AuthRequestModel auth)
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<string>>(x => x.Content == token && x.StatusCode == HttpStatusCode.OK);
            _client.Setup(x => x.ExecuteAsync<string>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            var actual = await _authRequestClient.GetToken(auth);

            Assert.AreEqual(token, actual);
            VerifyRequestTests<string>(_client);
        }

        #endregion

        #region send request to validate token test

        [Test]
        public async Task SendRequestToValidateTokenTest_WhenAuthServiceReturnedHttpCode200Ok()
        {
            IdentityResponseModel model = new IdentityResponseModel()
            {
                Id = 1,
                Role = Role.Admin.ToString(),
                IssuerMicroservice = Microservice.MarvelousConfigs.ToString()
            };

            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == model && x.StatusCode == HttpStatusCode.OK);
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            var actual = await _authRequestClient.SendRequestToValidateToken(token);

            VerifyRequestTests<IdentityResponseModel>(_client);
        }

        [Test]
        public async Task SendRequestToValidateTokenTest_WhenAuthServiceReturnedHttpCode401Unauthorized_ShouldThrowUnauthorizedException()
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode == 
            HttpStatusCode.Unauthorized && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<UnauthorizedException>(async () => await _authRequestClient.SendRequestToValidateToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
        }

        [Test]
        public async Task SendRequestToValidateTokenTest_WhenAuthServiceReturnedHttpCode403Forbidden_ShouldThrowForbiddenException()
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.Forbidden && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<ForbiddenException>(async () => await _authRequestClient.SendRequestToValidateToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
        }

        [Test]
        public async Task SendRequestToValidateTokenTest_WhenAuthServiceReturnedHttpCode400BadRequest_ShouldThrowBadRequestException()
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.BadRequest && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<BadRequestException>(async () => await _authRequestClient.SendRequestToValidateToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
        }

        [Test]
        public async Task SendRequestToValidateTokenTest_WhenAuthServiceReturnedHttpCode404NotFound_ShouldThrowEntityNotFoundException()
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.NotFound && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _authRequestClient.SendRequestToValidateToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
        }

        [Test]
        public async Task SendRequestToValidateTokenTest_WhenAuthServiceReturnedHttpCode409Conflict_ShouldThrowConflictException()
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.Conflict && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<ConflictException>(async () => await _authRequestClient.SendRequestToValidateToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
        }

        [Test]
        public async Task SendRequestToValidateTokenTest_WhenAuthServiceReturnedHttpCode422UnprocessableEntity_ShouldThrowValidationException()
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.UnprocessableEntity && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<ValidationException>(async () => await _authRequestClient.SendRequestToValidateToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
        }

        [Test]
        public async Task SendRequestToValidateTokenTest_WhenAuthServiceReturnedHttpCode503ServiceUnavailable_ShouldThrowServiceUnavailableException()
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.ServiceUnavailable && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<ServiceUnavailableException>(async () => await _authRequestClient.SendRequestToValidateToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
        }

        [Test]
        public async Task SendRequestToValidateTokenTest_WhenAuthServiceReturnedNotProvidedHttpCode_ShouldThrowServiceBadGatewayException()
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.AlreadyReported && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<BadGatewayException>(async () => await _authRequestClient.SendRequestToValidateToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
        }

        [Test]
        public async Task SendRequestToValidateTokenTest_WhenAuthServiceAnswerIsEmpty_ShouldThrowServiceBadGatewayException()
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == default && x.StatusCode ==
            HttpStatusCode.OK && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<BadGatewayException>(async () => await _authRequestClient.SendRequestToValidateToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
        }

        #endregion

        #region send request with token

        [Test]
        public async Task SendRequestWithTokenTest_WhenAuthServiceReturnedHttpCode200Ok()
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode == HttpStatusCode.OK);
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            await _authRequestClient.SendRequestWithToken(token);

            VerifyRequestTests<IdentityResponseModel>(_client);
        }

        [Test]
        public async Task SendRequestWithTokenTest_WhenAuthServiceReturnedHttpCode401Unauthorized_ShouldThrowUnauthorizedException()
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.Unauthorized && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<UnauthorizedException>(async () => await _authRequestClient.SendRequestWithToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);

        }

        [Test]
        public async Task SendRequestWithTokenTest_WhenAuthServiceReturnedHttpCode403Forbidden_ShouldThrowForbiddenException()
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.Forbidden && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<ForbiddenException>(async () => await _authRequestClient.SendRequestWithToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
        }

        [Test]
        public async Task SendRequestWithTokenTest_WhenAuthServiceReturnedHttpCode400BadRequest_ShouldThrowBadRequestException()
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.BadRequest && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<BadRequestException>(async () => await _authRequestClient.SendRequestWithToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
        }

        [Test]
        public async Task SendRequestWithTokenTest_WhenAuthServiceReturnedHttpCode404NotFound_ShouldThrowEntityNotFoundException()
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.NotFound && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _authRequestClient.SendRequestWithToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
        }

        [Test]
        public async Task SendRequestWithTokenTest_WhenAuthServiceReturned409Conflict_ShouldThrowConflictException()
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.Conflict && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<ConflictException>(async () => await _authRequestClient.SendRequestWithToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
        }

        [Test]
        public async Task SendRequestWithTokenTest_WhenAuthServiceReturnedHttpCode422UnprocessableEntity_ShouldThrowValidationException()
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.UnprocessableEntity && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<ValidationException>(async () => await _authRequestClient.SendRequestWithToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
        }

        [Test]
        public async Task SendRequestWithTokenTest_WhenAuthServiceReturnedHttpCode503ServiceUnavailable_ShouldThrowServiceUnavailableException()
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.ServiceUnavailable && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<ServiceUnavailableException>(async () => await _authRequestClient.SendRequestWithToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
        }

        [Test]
        public async Task SendRequestWithTokenTest_WhenAuthServiceReturnedNotProvidedHttpCode_ShouldThrowServiceBadGatewayException()
        {
            string token = "token";
            var responce = Mock.Of<RestResponse<IdentityResponseModel>>(x => x.Data == It.IsAny<IdentityResponseModel>() && x.StatusCode ==
            HttpStatusCode.BadGateway && x.ErrorException == new Exception(_mess));
            _client.Setup(x => x.ExecuteAsync<IdentityResponseModel>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(responce);

            Assert.ThrowsAsync<BadGatewayException>(async () => await _authRequestClient.SendRequestWithToken(token));
            VerifyRequestTests<IdentityResponseModel>(_client);
        }

        #endregion
    }
}
