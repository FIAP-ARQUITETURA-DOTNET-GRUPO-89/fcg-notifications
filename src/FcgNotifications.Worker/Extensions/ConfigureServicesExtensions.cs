using FcgNotifications.Infrastructure.Database;
using FcgNotifications.Infrastructure.Messaging;
using FcgNotifications.IoC;
using FcgNotifications.Worker.Consumers;
using Microsoft.EntityFrameworkCore;

namespace FcgNotifications.Worker.Extensions;

public static class ConfigureServicesExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureWorkerDependencies(configuration);

        services.AddDbContext<FcgNotificationsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default")));

        services.AddMassTransitRabbitMq(configuration, x =>
        {
            x.AddConsumer<PaymentProcessedConsumer>().Endpoint(e => e.Name = "notifications-payment-processed-events");
            x.AddConsumer<UserCreatedConsumer>().Endpoint(e => e.Name = "notifications-user-created-events");
        });

        return services;
    }
}
