namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Persistence;

internal static class ServiceCollectionExtensions {
    public static IHostApplicationBuilder AddPersistenceServices(this IHostApplicationBuilder builder) {
        builder.AddNpgsqlDbContext<PlanningPokerDataContext>("poker-data");

        return builder;
    }
}
