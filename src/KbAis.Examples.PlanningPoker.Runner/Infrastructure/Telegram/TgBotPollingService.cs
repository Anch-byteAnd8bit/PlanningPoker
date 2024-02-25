using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;

public class TgBotPollingService(IServiceProvider services, ILogger<TgBotPollingService> logger) : BackgroundService {
    protected override async Task ExecuteAsync(Cancellation c) {
        // TODO: Wrap into polly's policy
        while (c.IsCancellationRequested == false) {
            try {
                using var updServiceScope = services.CreateScope();

                var updateReceiver = updServiceScope.ServiceProvider
                    .GetRequiredService<TgUpdateReceiveService>();

                logger.LogDebug("Polling next telegram update");

                await updateReceiver.ReceiveAsync(c);
            } catch (Exception exception) {
                logger.LogError(exception, "Caught exception on telegram update polling");
            }
        }
    }
}
