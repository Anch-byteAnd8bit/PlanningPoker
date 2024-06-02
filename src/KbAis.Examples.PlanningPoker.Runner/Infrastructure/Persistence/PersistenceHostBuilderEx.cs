namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Persistence;

internal static class PersistenceHostBuilderEx {
    public static IHostApplicationBuilder AddPlanningPokerPersistenceServices(this IHostApplicationBuilder builder) {
        builder.AddNpgsqlDbContext<PlanningPokerDataContext>("poker-data");

        return builder;
    }
}
