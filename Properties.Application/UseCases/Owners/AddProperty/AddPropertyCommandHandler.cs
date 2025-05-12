using Crosscutting;

using MediatR;

using Properties.Application.Interfaces;
using Properties.Domain.Entities;
using Properties.Domain.Errors;

namespace Properties.Application.UseCases.Owners.AddProperty
{
    public class AddPropertyCommandHandler(
        IOwnerRepository ownerRepository,
        IPropertyRepository propertyRepository,
        IPropertyTraceRepository propertyTraceRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<AddPropertyCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(AddPropertyCommand request, CancellationToken cancellationToken)
        {
            var owner = await ownerRepository.GetByIdAsync(request.OwnerId, true);
            if (owner is null)
                return Result.Failure<Guid>(OwnerError.NotFoundById);

            var property = await propertyRepository.GetByIdAsync(request.PropertyId);
            if (property is null)
                return Result.Failure<Guid>(PropertyError.NotFoundById);

            var newPropertyTrace = PropertyTrace.Create(property.Id, owner.Name, DateTime.UtcNow, request.Trace.Value, request.Trace.Tax);

            property.ChangeOwner(owner.Id);
            await propertyTraceRepository.CreateAsync(newPropertyTrace);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return request.OwnerId;
        }
    }
}
