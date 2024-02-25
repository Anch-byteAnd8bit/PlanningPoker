namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;

public sealed class TgBotOptions {
    public const string ConfigurationSectionPath = "Services:TelegramBot";

    public required string Token { get; init; }
}