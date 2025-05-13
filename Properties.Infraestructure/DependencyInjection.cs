using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Properties.Application.Interfaces;
using Properties.Infraestructure.DataBase;
using Properties.Infraestructure.Repositories;
using Properties.Infraestructure.Services;

using System.Text;

namespace Properties.Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
            services
            .AddServices()
            .AddDatabase(configuration)
            .AddHealthChecks(configuration)
            .AddAuthenticationInternal(configuration);

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IOwnerRepository, OwnerRepository>();
            services.AddScoped<IPropertyTraceRepository, PropertyTraceRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IBlobStorageService, BlobStorageService>();           

            return services;
        }

        private static IServiceCollection AddAuthenticationInternal(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)),
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<ITokenProvider, TokenProvider>();
            services.AddAuthorization();

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
