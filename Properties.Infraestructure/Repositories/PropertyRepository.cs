
using Microsoft.EntityFrameworkCore;

using Properties.Application.Interfaces;
using Properties.Domain.Entities;
using Properties.Infraestructure.DataBase;

namespace Properties.Infraestructure.Repositories
{
    public class PropertyRepository(ApplicationDbContext context) : IPropertyRepository
    {
        public async Task<Property> CreateAsync(Property property)
        {
            await context.Properties.AddAsync(property);
            return property;
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Property>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Property> GetAll(bool asNoTracking = false)
        {
            return asNoTracking ? context.Properties.AsNoTracking().AsQueryable() : 
                context.Properties.AsQueryable();
        }

        public async Task<Property?> GetByIdAsync(Guid id, bool asNoTracking = false)
        {
            return asNoTracking ? await context
                .Properties
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id) : 
                await context.Properties.FirstOrDefaultAsync(x => x.Id == id);
        }
        public void Update(Property property) => context.Update(property);
    }
}
