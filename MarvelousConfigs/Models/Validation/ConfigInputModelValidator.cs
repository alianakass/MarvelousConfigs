using FluentValidation;

namespace MarvelousConfigs.API.Models.Validation
{
    public class ConfigInputModelValidator : AbstractValidator<ConfigInputModel>
    {
        public ConfigInputModelValidator()
        {
            RuleFor(t => t.Value).NotEmpty();
        }
    }
}
