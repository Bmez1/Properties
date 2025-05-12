using FluentValidation;

namespace Properties.Application.UseCases.Properties.Create
{
    internal sealed class CreatePropertyValidation : AbstractValidator<CreatePropertyCommand>
    {
        public CreatePropertyValidation()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Year).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0);

            When(x => x.OwnerId.HasValue, () =>
            {
                RuleFor(x => x.Trace)
                    .NotNull()
                    .SetValidator(new ProertyTraceCreateValidation());
            });
        }
    }
}
