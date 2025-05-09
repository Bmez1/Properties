using Properties.Application.Interfaces;
using Properties.Infraestructure.DataBase;

namespace Properties.Infraestructure.Repositories
{
    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
