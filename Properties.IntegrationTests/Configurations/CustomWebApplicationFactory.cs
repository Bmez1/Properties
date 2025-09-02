using Bogus;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Properties.Domain.Entities;
using Properties.Infraestructure.DataBase;

namespace Properties.IntegrationTests.Configurations;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "IntegrationTesting");
        builder.ConfigureServices(services =>
        {
            Console.WriteLine("Configuring services for testing...");

            // Remove ALL AppDbContext registrations
            var descriptors = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>))
                .ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            var appDbContext = services.SingleOrDefault(
                d => d.ServiceType == typeof(ApplicationDbContext));
            if (appDbContext != null)
                services.Remove(appDbContext);

            // Register AppDbContext using EF Core InMemory provider
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });
            Console.WriteLine("Added EF Core InMemory database.");

            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            db.Database.EnsureCreated();

            SeedDatabase(db);

        });

    }

    private static void SeedDatabase(ApplicationDbContext context)
    {
        // Limpiar la base de datos
        context.Users.RemoveRange(context.Users);
        context.Owners.RemoveRange(context.Owners);

        User user = User.Create("mail@mail.com", "12345678");
        
        // Agregar datos de prueba
        context.Users.Add(user);
        context.Owners.AddRange(CreateOwners(2));

        context.SaveChanges();
    }

    private static IEnumerable<Owner> CreateOwners(int count)
    {
        var owners = new Faker<Owner>()
            .CustomInstantiator(f => Owner.Create
            (
                f.Name.FirstName(),
                f.Address.StreetAddress(),
                f.Date.PastDateOnly()
            ))
            .Generate(count);

        return owners;
    }
}
