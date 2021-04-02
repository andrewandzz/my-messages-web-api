using FluentValidation;
using MyMessages.Api.Models;

namespace MyMessages.Api.Validation
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(model => model.Name)
                .NotEmpty()
                .WithMessage(model => $"Value of property '{nameof(model.Name)}' cannot be empty.");

            RuleFor(model => model.Password)
                .NotEmpty()
                .WithMessage(model => $"Value of property '{nameof(model.Password)}' cannot be empty.");
        }
    }
}
