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
                if (request.OwnerId.HasValue && !await ownerRepository.ExistsByIdAsync(request.OwnerId.Value))
                    return Result.Failure<Guid>(OwnerError.NotFoundById);

                var trace = PropertyTrace.Create
                (
                    request.PropertyId,
                    request.OwnerId.HasValue? request.Name : "Ownerless",
                    DateTime.UtcNow,
                    request.Trace?.Value ?? property.Price,
                    0
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
