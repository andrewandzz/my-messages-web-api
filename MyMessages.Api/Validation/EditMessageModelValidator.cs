using FluentValidation;
using MyMessages.Api.Models;

namespace MyMessages.Api.Validation
{
    public class EditMessageModelValidator : AbstractValidator<EditMessageModel>
    {
        public EditMessageModelValidator()
        {
            RuleFor(model => model.Text)
                .NotEmpty()
                .When(model => model.Picture == null && model.File == null)
                .WithMessage("All the properties were null or empty. At least one property must be set.");

            RuleFor(model => model.Picture)
                .NotNull()
                .When(model => string.IsNullOrEmpty(model.Text?.Trim()) && model.File == null)
                .WithMessage("All the properties were null or empty. At least one property must be set.");

            RuleFor(model => model.File)
                .NotNull()
                .When(model => string.IsNullOrEmpty(model.Text?.Trim()) && model.Picture == null)
                .WithMessage("All the properties were null or empty. At least one property must be set.");
        }
    }
}
