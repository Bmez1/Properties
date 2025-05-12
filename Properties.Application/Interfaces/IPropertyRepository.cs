using Properties.Domain.Entities;

namespace Properties.Application.Interfaces
{
    public interface IPropertyRepository
    {
        IQueryable<Property> GetAll(bool asNoTracking = false);
        Task<Property?> GetByIdAsync(Guid id, bool asNoTracking = false);
        Task<Property> CreateAsync(Property property);
        void Update(Property property);
        Task<bool> ExistsByIdAsync(Guid propertyId);
    }
}
