using Microsoft.Extensions.Hosting;
using PlanningPoker;

var hostBuilder = Host.CreateEmptyApplicationBuilder(new() { Args = args });

var host = hostBuilder.Build();

if (hostBuilder.Configuration["Bot:Token"] is { } token)
{
    var bot = new Bot();
    bot.ConnectBotAsync(token).Wait();
    host.Run();
}
else Console.WriteLine("Не удалось получить Token");
