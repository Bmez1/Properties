using FluentValidation;

using Properties.Application.UseCases.Properties.Create;

namespace Properties.Application.UseCases.Owners.AddProperty
{
    public class AddPropertyValidation : AbstractValidator<AddPropertyCommand>
    {
        public AddPropertyValidation()
        {
            RuleFor(x => x.OwnerId).NotEmpty();
            RuleFor(x => x.PropertyId).NotEmpty();

            RuleFor(x => x.Trace)
            .NotNull()
            .SetValidator(new ProertyTraceCreateValidation());
        }
    }

}
