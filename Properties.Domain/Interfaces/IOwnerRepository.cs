using Properties.Domain.Entities;

namespace Properties.Domain.Interfaces
{
    public interface IOwnerRepository
    {
        Task<Owner> CreateAsync(Owner owner);
    }
}
