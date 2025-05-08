using FluentValidation;

namespace Properties.Application.UseCases.Owners.Create
{
    internal sealed class CreateOwnerValidation : AbstractValidator<CreateOwnerCommand>
    {
        public CreateOwnerValidation()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Birthday).NotEmpty();
        }
    }

}
