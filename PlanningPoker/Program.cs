using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PlanningPoker;
using PlanningPoker.Models;

var hostBuilder = Host.CreateEmptyApplicationBuilder(new() { Args = args });

hostBuilder.Logging
    .SetMinimumLevel(LogLevel.Debug)
    .AddConsole();

hostBuilder.Configuration
    .AddJsonFile("appsettings.json", optional: false);

hostBuilder.Services
    .AddTelegramBotService();

hostBuilder.Services.AddTransient<SampleServ>();

hostBuilder.Services.AddDbContext<PockerDBContext>(options => options.UseNpgsql(hostBuilder.Configuration.GetConnectionString("DefaultConnection")));

var host = hostBuilder.Build();

host.Services.GetRequiredService<ILogger<Program>>().LogInformation("Starting up Poker service");

host.Run();

public sealed class TelegramBotOptions
{
    public const string ConfigurationSectionPath = "Services:TelegramBot";

    public required string Token { get; init; }
}
