using FluentValidation;

using Properties.Application.UseCases.Properties.Create;

namespace Properties.Application.UseCases.Owners.AddProperty
{
    public class AddPropertyValidator : AbstractValidator<AddPropertyCommand>
    {
        public AddPropertyValidator()
        {
            RuleFor(x => x.OwnerId).NotEmpty();
            RuleFor(x => x.PropertyId).NotEmpty();

            RuleFor(x => x.Trace)
            .NotNull()
            .SetValidator(new PropertyTraceCreateValidator());
        }
    }

}
