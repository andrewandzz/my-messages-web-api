using FluentValidation;
using MyMessages.Api.Models;

namespace MyMessages.Api.Validation
{
    public class NewMessageModelValidator : AbstractValidator<NewMessageModel>
    {
        public NewMessageModelValidator()
        {
            RuleFor(model => model.Text)
                .NotEmpty()
                .When(model => model.Picture == null && model.File == null && model.StickerId == null)
                .WithMessage("All the properties were null or empty. At least one property must be set.");

            RuleFor(model => model.Picture)
                .NotNull()
                .When(model => string.IsNullOrEmpty(model.Text?.Trim()) && model.File == null && model.StickerId == null)
                .WithMessage("All the properties were null or empty. At least one property must be set.");

            RuleFor(model => model.File)
                .NotNull()
                .When(model => string.IsNullOrEmpty(model.Text?.Trim()) && model.Picture == null && model.StickerId == null)
                .WithMessage("All the properties were null or empty. At least one property must be set.");

            RuleFor(model => model.Text)
                .Empty()
                .When(model => model.StickerId != null)
                .WithMessage(model => $"When '{nameof(model.StickerId)}' is set, all the other properties should by empty.");

            RuleFor(model => model.Picture)
                .Null()
                .When(model => model.StickerId != null)
                .WithMessage(model => $"When '{nameof(model.StickerId)}' is set, all the other properties should by empty.");

            RuleFor(model => model.File)
                .Null()
                .When(model => model.StickerId != null)
                .WithMessage(model => $"When '{nameof(model.StickerId)}' is set, all the other properties should by empty.");
        }
    }
}
