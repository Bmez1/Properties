using FluentValidation;

using Properties.Application.UseCases.Properties.Dtos;

namespace Properties.Application.UseCases.Properties.Create
{
    internal class ProertyTraceCreateValidation : AbstractValidator<PropertyTraceCreateDto>
    {
        public ProertyTraceCreateValidation()
        {
            RuleFor(x => x.Value).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Tax).GreaterThanOrEqualTo(0);
        }
    }
}
