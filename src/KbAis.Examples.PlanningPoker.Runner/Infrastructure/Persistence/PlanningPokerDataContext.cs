using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Persistence;

public class PlanningPokerDataContext(IConfiguration configuration) : DbContext {
    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
    }
}
