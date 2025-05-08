using Properties.Domain.Entities;

namespace Properties.Domain.Interfaces
{
    public interface IPropertyRepository
    {
        Task<IEnumerable<Property>> GetAllAsync();
        Task<Property> GetByIdAsync(Guid id);
        Task<Property> CreateAsync(Property property);
        Task<Property> UpdateAsync(Property property);
        Task DeleteAsync(int id);
    }
}
