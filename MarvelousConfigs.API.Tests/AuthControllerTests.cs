using FluentValidation;
using Marvelous.Contracts.RequestModels;
using MarvelousConfigs.API.Controllers;
using MarvelousConfigs.API.Models.Validation;
using MarvelousConfigs.BLL.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MarvelousConfigs.API.Tests
{
    public class AuthControllerTests : BaseVerifyTest<AuthController>
    {
        private Mock<IAuthRequestClient> _auth;
        private AuthController _controller;
        private IValidator<AuthRequestModel> _validator;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<AuthController>>();
            _auth = new Mock<IAuthRequestClient>();
            _validator = new AuthRequestModelValidator();
            _controller = new AuthController(_logger.Object, _auth.Object, _validator);
        }

        [TestCaseSource(typeof(LoginTestCaseSource))]
        public async Task LoginTest_Should200Ok(AuthRequestModel model)
        {
            var token = "token";
            _auth.Setup(x => x.GetToken(model)).ReturnsAsync(token);

            await _controller.Login(model);

            _auth.Verify(x => x.GetToken(model), Times.Once);
        }
    }
}
