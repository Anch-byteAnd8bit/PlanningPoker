using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlanningPoker;

var hostBuilder = Host.CreateEmptyApplicationBuilder(new() { Args = args });

hostBuilder.Configuration.AddJsonFile("appsettings.json", optional: false);

hostBuilder.Services.AddTelegramBotService();

var host = hostBuilder.Build();

if (hostBuilder.Configuration["Bot:Token"] is { } token)
{
    var bot = new Bot();
    bot.ConnectBotAsync(token).Wait();
    host.Run();
}
else Console.WriteLine("Не удалось получить Token");

public sealed class TelegramBotOptions {
    public const string ConfigurationSectionPath = "Services:TelegramBot";

    public required string Token { get; init; }
}

public static class ServiceCollectionExtensions {
    public static void AddTelegramBotService(this IServiceCollection services) {
        services.AddOptions<TelegramBotOptions>()
            .BindConfiguration(TelegramBotOptions.ConfigurationSectionPath);
    }
}
