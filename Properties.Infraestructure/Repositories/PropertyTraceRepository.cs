using Properties.Application.Interfaces;
using Properties.Domain.Entities;
using Properties.Infraestructure.DataBase;

namespace Properties.Infraestructure.Repositories
{
    public class PropertyTraceRepository(ApplicationDbContext context) : IPropertyTraceRepository
    {
        public async Task CreateAsync(PropertyTrace trace)
        {
            await context.PropertyTraces.AddAsync(trace);
        }
    }
}
