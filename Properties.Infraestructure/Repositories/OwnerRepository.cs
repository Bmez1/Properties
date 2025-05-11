
using Microsoft.EntityFrameworkCore;

using Properties.Application.Interfaces;
using Properties.Domain.Entities;
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

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await context
                .Owners
                .AsNoTracking()
                .AnyAsync(x => x.Id == id);
        }
    }
}
