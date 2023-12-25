using PlanningPoker;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
if (builder.Configuration["Bot:Token"] is { } token)
{
    var bot = new Bot();
    bot.ConnectBotAsync(token).Wait();
    app.Run();
}
else Console.WriteLine("Не удалось получить Token");