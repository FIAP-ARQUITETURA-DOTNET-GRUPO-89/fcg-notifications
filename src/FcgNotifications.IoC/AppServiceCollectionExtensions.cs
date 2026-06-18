using FluentValidation;
using MediatR;
using FcgNotifications.Application;
using FcgNotifications.Domain;
using FcgNotifications.Domain.Repositories.Orders;
using FcgNotifications.Infrastructure.Database;
using FcgNotifications.Infrastructure.Repositories.Orders;
using FcgNotifications.SharedKernel.Behaviors;
using FcgNotifications.SharedKernel.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FcgNotifications.IoC;

public static class AppServiceCollectionExtensions
{
    public static void ConfigureAppDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtSettings>().Bind(configuration.GetSection("JwtSettings"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(
                typeof(IDomainEntryPoint).Assembly,
                typeof(IApplicationAssembly).Assembly,
                typeof(ValidationBehavior<,>).Assembly)
        );

        services.AddValidatorsFromAssemblyContaining<IApplicationAssembly>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddDbContext<FcgNotificationsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default"),
                npgsql => npgsql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorCodesToAdd: null)));

        // Repositories
        services.AddScoped<IOrderRepository, OrderRepository>();

        // Services

    }
}
