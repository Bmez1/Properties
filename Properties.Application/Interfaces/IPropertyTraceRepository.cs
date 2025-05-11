using Properties.Domain.Entities;

namespace Properties.Application.Interfaces
{
    public interface IPropertyTraceRepository
    {
        Task CreateAsync(PropertyTrace trace);
    }
}
