using FluentValidation;

using Properties.Application.UseCases.Properties.Create;

namespace Properties.Application.UseCases.Properties.Update
{
    internal class UpdatePropertyCommadValidator : AbstractValidator<UpdatePropertyCommad>
    {
        public UpdatePropertyCommadValidator()
        {
            RuleFor(x => x.PropertyId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.Year).NotEmpty();

            When(x => x.Trace is not null, () =>
            {
                RuleFor(x => x.Trace)
                    .SetValidator(new PropertyTraceCreateValidator());
            });
        }
    }
}
