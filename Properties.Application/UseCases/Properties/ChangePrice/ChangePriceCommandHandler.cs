using Crosscutting;

using MediatR;

using Properties.Application.Interfaces;
using Properties.Domain.Errors;

namespace Properties.Application.UseCases.Properties.ChangePrice
{
    public class ChangePriceCommandHandler(IPropertyRepository propertyRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<ChangePriceCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(ChangePriceCommand request, CancellationToken cancellationToken)
        {
            var property = await propertyRepository.GetByIdAsync(request.PropertyId);

            if (property is null)
                return Result.Failure<Guid>(PropertyError.NotFoundById);

            property.UpdatePrice(request.Price);

            propertyRepository.Update(property);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return property.Id;
        }
    }
}
