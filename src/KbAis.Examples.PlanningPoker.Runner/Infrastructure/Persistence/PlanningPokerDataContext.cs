using KbAis.Examples.PlanningPoker.Runner.Domain.Projects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Telegram.Bot.Types;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Persistence;

public class PlanningPokerDataContext(IConfiguration configuration) : DbContext {
    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
    }
    public DbSet<outbox_table> outbox_table { get; set; }
    public DbSet<ProjectForDB> projects { get; set; }
    public DbSet<SprintForDB> sprints { get; set; }
    public DbSet<TaskForDB> task { get; set; }
    public DbSet<MemberForDB> members { get;}
    
}
