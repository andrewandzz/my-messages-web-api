using FluentValidation;
using MyMessages.Api.Models;

namespace MyMessages.Api.Validation
{
    public class RegisterModelValidator : AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator()
        {
            RuleFor(model => model.Name)
                .NotEmpty()
                    .WithMessage(model => $"Property '{nameof(model.Name)}' cannot be empty.")
                .MinimumLength(3)
                    .WithMessage("Name length should be more than or equal to 3.")
                .MaximumLength(30)
                    .WithMessage("Name length cannot be more than 30 characters long.");

            RuleFor(model => model.Password)
                .NotEmpty()
                    .WithMessage(model => $"Property '{nameof(model.Password)}' cannot be empty.")
                .MinimumLength(6)
                    .WithMessage("Password length should be more than or equal to 6.");
        }
    }
}
