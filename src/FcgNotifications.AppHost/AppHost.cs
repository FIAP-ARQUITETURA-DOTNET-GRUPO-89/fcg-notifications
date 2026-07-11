using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);
var isTesting = builder.Environment.IsEnvironment("Testing");

var postgresPassword = builder.AddParameter("postgresPassword", "postgres");

var postgres = isTesting
    ? builder.AddPostgres("Postgres")
        .WithPassword(postgresPassword)
        .WithLifetime(ContainerLifetime.Session)
        .AddDatabase("Default", "fcgnotifications-db")
    : builder.AddPostgres("Postgres", port: 5432)
        .WithPassword(postgresPassword)
        .WithLifetime(ContainerLifetime.Persistent)
        .WithPgAdmin(c => c.WithLifetime(ContainerLifetime.Persistent))
        .AddDatabase("Default", "fcgnotifications-db");

var rabbitmq = isTesting
    ? builder.AddRabbitMQ("rabbitmq")
        .WithManagementPlugin()
        .WithLifetime(ContainerLifetime.Session)
    : builder.AddRabbitMQ("rabbitmq")
        .WithManagementPlugin()
        .WithLifetime(ContainerLifetime.Persistent);

builder.AddProject<Projects.FcgNotifications_Worker>("fcgnotifications-worker")
       .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
       .WithReference(postgres)
       .WithReference(rabbitmq)
       .WaitFor(postgres)
       .WaitFor(rabbitmq);

builder.Build().Run();
