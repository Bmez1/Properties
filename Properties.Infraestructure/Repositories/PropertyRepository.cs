
using Properties.Domain.Entities;
using Properties.Domain.Interfaces;
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

        public Task<Property> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Property> UpdateAsync(Property property)
        {
            throw new NotImplementedException();
        }
    }
}
