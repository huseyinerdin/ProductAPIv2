using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductAPI.Infrastructure.Data;
using ProductAPI.Infrastructure.Helpers;
using ProductAPI.Infrastructure.Repositories;

namespace ProductAPI.Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokenHandler, TokenHandler>();
            return services;
        }
    }
}
