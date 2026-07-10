using FcgNotifications.IoC;
using FcgNotifications.Infrastructure.Messaging;
using FcgNotifications.Worker.Consumers;

namespace FcgNotifications.Worker.Extensions;

public static class ConfigureServicesExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureWorkerDependencies(configuration);

        services.AddMassTransitRabbitMq(configuration, x =>
        {
            x.AddConsumer<PaymentProcessedConsumer>();
            x.AddConsumer<UserCreatedConsumer>();
        });

        return services;
    }
}
