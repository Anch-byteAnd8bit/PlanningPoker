using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Persistence;

[SuppressMessage("ReSharper", "EntityFramework.ModelValidation.UnlimitedStringLength")]
public class ProjectSample {
    public long Id { get; set; }

    public string? Name { get; set; }
}

public class PlanningPokerDataContext(DbContextOptions opts) : DbContext(opts) {

    protected override void OnConfiguring(DbContextOptionsBuilder options) { }

    protected override void OnModelCreating(ModelBuilder builder) {
        builder.UseIdentityByDefaultColumns();

        builder.Entity<ProjectSample>(b => {
            b.ToTable("projects");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id).HasColumnName("id");

            b.Property(x => x.Name).HasColumnName("name")
                .HasMaxLength(250).IsRequired();
        });
    }

}
