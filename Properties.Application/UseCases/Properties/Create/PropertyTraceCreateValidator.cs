using FluentValidation;

using Properties.Application.UseCases.Properties.Dtos;

namespace Properties.Application.UseCases.Properties.Create
{
    internal class PropertyTraceCreateValidator : AbstractValidator<PropertyTraceCreateDto>
    {
        public PropertyTraceCreateValidator()
        {
            RuleFor(x => x.Value).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Tax).GreaterThanOrEqualTo(0);
        }
    }
}
