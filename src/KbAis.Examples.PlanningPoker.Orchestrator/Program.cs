using Microsoft.Extensions.Configuration;
using Projects;

var hostBuilder = DistributedApplication.CreateBuilder(args);

hostBuilder.Configuration.AddInMemoryCollection(new Dictionary<string, string?> {
    ["Parameters:PokerDatabase-Username"] = "dev",
    ["Parameters:PokerDatabase-Password"] = "dev123",
});

var pokerDataResource = hostBuilder
    .AddPostgres(
        "poker-data",
        userName: hostBuilder.AddParameter("PokerDatabase-Username"),
        password: hostBuilder.AddParameter("PokerDatabase-Password"),
        6000
    )
    .WithDataVolume();

var pokerDatabaseResource = pokerDataResource.AddDatabase("poker-data-db");

var pokerServiceResource = hostBuilder
    .AddProject<KbAis_Examples_PlanningPoker_Runner>("poker-service")
    .WithReference(pokerDataResource)
    .WithExternalHttpEndpoints();

await using var host = hostBuilder.Build();

await host.RunAsync();
