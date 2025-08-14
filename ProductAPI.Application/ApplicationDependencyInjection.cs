using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductAPI.Application.CQRS.Products.Queries;
using ProductAPI.Application.Mapping;
using ProductAPI.Application.Services;
using ProductAPI.Infrastructure;
using System.Reflection;

namespace ProductAPI.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructureServices(configuration);
            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.RegisterServicesFromAssembly(typeof(GetProductsQuery).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ProductProfile).Assembly);
            });
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ProductProfile>();
            }, Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
