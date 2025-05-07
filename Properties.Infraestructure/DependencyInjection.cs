using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Properties.Infraestructure.DataBase;

namespace Properties.Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
            services
            .AddServices()
            .AddDatabase(configuration)
            .AddHealthChecks(configuration);

        private static IServiceCollection AddServices(this IServiceCollection services)
        {


            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(
                options => options
                    .UseSqlServer(connectionString));

            return services;
        }

        private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("DefaultConnection")!);

            return services;
        }
    }
}
