using FluentValidation;

namespace Properties.Application.UseCases.Properties.ChangePrice
{
    public class ChangePriceCommandValidator : AbstractValidator<ChangePriceCommand>
    {
        public ChangePriceCommandValidator()
        {
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }
}
