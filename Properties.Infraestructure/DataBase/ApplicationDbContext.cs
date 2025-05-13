using Microsoft.EntityFrameworkCore;

using Properties.Domain.Entities;

namespace Properties.Infraestructure.DataBase
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyTrace> PropertyTraces { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
