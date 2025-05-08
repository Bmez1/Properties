
using Properties.Domain.Entities;
using Properties.Domain.Interfaces;
using Properties.Infraestructure.DataBase;

namespace Properties.Infraestructure.Repositories
{
    public class OwnerRepository(ApplicationDbContext context) : IOwnerRepository
    {
        public async Task<Owner> CreateAsync(Owner owner)
        {
            await context.Owners.AddAsync(owner);
            return owner;
        }
    }

}
