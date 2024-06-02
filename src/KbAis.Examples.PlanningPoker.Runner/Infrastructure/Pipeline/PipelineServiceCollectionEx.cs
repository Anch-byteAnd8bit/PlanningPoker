using System.Reflection;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Pipeline;

internal static class PipelineServiceCollectionEx {
    public static IServiceCollection AddPlanningPokerPipeline(this IServiceCollection services) {
        services.AddMediatR(config => {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            config.AddOpenBehavior(typeof(UnitOfWorkBehaviour<,>));
        });

        return services;
    }
}