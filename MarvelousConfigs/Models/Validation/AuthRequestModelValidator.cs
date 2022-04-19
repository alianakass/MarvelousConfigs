using FluentValidation;
using Marvelous.Contracts.RequestModels;

namespace MarvelousConfigs.API.Models.Validation
{
    public class AuthRequestModelValidator : AbstractValidator<AuthRequestModel>
    {
        public AuthRequestModelValidator()
        {
            RuleFor(t => t.Email).NotEmpty();
            RuleFor(t => t.Password).NotEmpty();
        }

    }
}
