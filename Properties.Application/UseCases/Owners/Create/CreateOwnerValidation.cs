using FluentValidation;

using Properties.Application.UseCases.Properties.Create;

namespace Properties.Application.UseCases.Owners.Create
{
    internal sealed class CreateOwnerValidation : AbstractValidator<CreateOwnerCommand>
    {
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif"];
        private const long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5MB

        public CreateOwnerValidation()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Birthday).NotEmpty();

            When(x => x.Photo is not null, () =>
            {
                RuleFor(x => x.Photo!.Extension)
                    .Must(ext => AllowedExtensions.Contains(ext.ToLowerInvariant()))
                    .WithMessage($"Only image files with the following extensions are allowed: {string.Join(", ", AllowedExtensions)}");

                RuleFor(x => x.Photo!.Size)
                    .LessThanOrEqualTo(MaxFileSizeInBytes)
                    .WithMessage($"The maximum allowed file size is {MaxFileSizeInBytes / (1024 * 1024)} MB.");
            });
        }    }

}
