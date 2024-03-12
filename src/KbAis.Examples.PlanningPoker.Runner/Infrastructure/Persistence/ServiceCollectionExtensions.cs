using Microsoft.Extensions.DependencyInjection;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Persistence;

internal static class ServiceCollectionExtensions {
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services) {
        services.AddDbContext<PlanningPokerDataContext>();
        return services;
    }
}
