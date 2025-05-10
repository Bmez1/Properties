using FluentValidation;

namespace Properties.Application.UseCases.Properties.AddImage
{
    public class AddImageCommandValidator : AbstractValidator<AddImageCommand>
    {
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif"];
        private const long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5MB

        public AddImageCommandValidator()
        {
            RuleFor(x => x.PropertyId)
                .NotEmpty()
                .WithMessage("The property ID is required.");

            RuleFor(x => x.FileUpload)
                .NotNull().WithMessage("You must provide an image.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.FileUpload.Extension)
                        .Must(ext => AllowedExtensions.Contains(ext.ToLowerInvariant()))
                        .WithMessage($"Only image files with the following extensions are allowed: {string.Join(", ", AllowedExtensions)}");

                    RuleFor(x => x.FileUpload.Size)
                        .LessThanOrEqualTo(MaxFileSizeInBytes)
                        .WithMessage($"The maximum allowed file size is {MaxFileSizeInBytes / (1024 * 1024)} MB.");
                });

        }
    }
}
