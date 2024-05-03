using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;

public class TgUpdatePollingService(
    IServiceProvider services, ILogger<TgUpdatePollingService> logger
) : BackgroundService {
    protected override async Task ExecuteAsync(Cancellation c) {
        // TODO: Wrap into polly's policy
        while (c.IsCancellationRequested == false) {
            try {
                using var tgUpdateServiceScope = services.CreateScope();

                var updateReceiver = tgUpdateServiceScope.ServiceProvider
                    .GetRequiredService<TgUpdateReceiveService>();

                logger.LogDebug("Polling the next Telegram update");

                await updateReceiver.ReceiveAsync(c);
            } catch (Exception exception) {
                logger.LogError(exception, "Caught an exception while polling Telegram update");
            }
        }
    }
}
