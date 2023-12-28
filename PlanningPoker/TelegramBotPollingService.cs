using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PlanningPoker;

public class TelegramBotPollingService(
    IServiceProvider services, ILogger<TelegramBotPollingService> logger
) : BackgroundService {
    protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
        // TODO: Wrap into polly's policy
        while (cancellationToken.IsCancellationRequested == false) {
            try {
                using var updateServiceScope = services.CreateScope();

                var updateReceiver = updateServiceScope.ServiceProvider
                    .GetRequiredService<TelegramUpdateReceiveService>();

                logger.LogDebug("Polling next telegram update");

                await updateReceiver.ReceiveAsync(cancellationToken);
            } catch (Exception exception) {
                logger.LogError(exception, "Caught exception on telegram update polling");
            }
        }
    }
}
