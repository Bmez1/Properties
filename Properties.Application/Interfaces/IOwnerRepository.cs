using Properties.Domain.Entities;

namespace Properties.Application.Interfaces
{
    public interface IOwnerRepository
    {
        Task<Owner> CreateAsync(Owner owner);
        Task<bool> ExistsByIdAsync(Guid id);
    }

}
