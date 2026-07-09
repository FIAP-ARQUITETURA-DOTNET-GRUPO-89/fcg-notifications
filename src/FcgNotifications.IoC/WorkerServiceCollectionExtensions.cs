using FcgNotifications.Application;
using FcgNotifications.Domain;
using FcgNotifications.Domain.Repositories;
using FcgNotifications.Infrastructure.Database;
using FcgNotifications.Infrastructure.Repositories;
using FcgNotifications.SharedKernel.Behaviors;
using FcgNotifications.SharedKernel.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FcgNotifications.IoC;

public static class WorkerServiceCollectionExtensions
{
    public static void ConfigureWorkerDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<MassTransitSettings>().Bind(configuration.GetSection("MassTransit"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(
                typeof(IDomainEntryPoint).Assembly,
                typeof(IApplicationAssembly).Assembly,
                typeof(ValidationBehavior<,>).Assembly)
        );

        // Banco
        services.AddDbContext<FcgNotificationsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default"),
                npgsql => npgsql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorCodesToAdd: null)));

        // Repositories
        services.AddScoped<INotificationRepository, NotificationRepository>(); // Registrado

        // Services
    }
}
