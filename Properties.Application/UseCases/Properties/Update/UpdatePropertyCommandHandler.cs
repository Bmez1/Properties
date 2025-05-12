using Crosscutting;

using MediatR;

using Properties.Application.Interfaces;
using Properties.Domain.Entities;
using Properties.Domain.Errors;

namespace Properties.Application.UseCases.Properties.Update
{
    public class UpdatePropertyCommandHandler(
        IPropertyRepository propertyRepository,
        IOwnerRepository ownerRepository,
        IPropertyTraceRepository propertyTraceRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<UpdatePropertyCommad, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(UpdatePropertyCommad request, CancellationToken cancellationToken)
        {
            var property = await propertyRepository.GetByIdAsync(request.PropertyId, true);

            if (property is null)
                return Result.Failure<Guid>(PropertyError.NotFoundById);

            if (property.IsChangeOwner(request.OwnerId))
            {
                var ownerName = string.Empty;
                if (request.OwnerId.HasValue)
                {
                    var owner = await ownerRepository.GetByIdAsync(request.OwnerId.Value);
                    if (owner is null)
                        return Result.Failure<Guid>(OwnerError.NotFoundById);
                        
                    ownerName = owner.Name;                    
                }

                var trace = PropertyTrace.Create
                (
                    request.PropertyId,
                    request.OwnerId.HasValue? ownerName : "Ownerless",
                    DateTime.UtcNow,
                    request.Trace?.Value ?? property.Price,
                    request.Trace?.Tax ?? 0
                );

                await propertyTraceRepository.CreateAsync(trace);
            }

            property.Update(request.Name, request.Address, request.Price, request.Year, request.OwnerId);

            propertyRepository.Update(property);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return property.Id;
        }
    }
}
