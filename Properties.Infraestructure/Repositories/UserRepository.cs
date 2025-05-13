using Microsoft.EntityFrameworkCore;

using Properties.Application.Interfaces;
using Properties.Domain.Entities;
using Properties.Infraestructure.DataBase;

namespace Properties.Infraestructure.Repositories
{
    public class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        public async Task AddAsync(User user)
        {
            await context.AddAsync(user);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await context.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
        }
    }
}
