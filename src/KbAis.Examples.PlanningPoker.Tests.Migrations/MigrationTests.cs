using Dapper;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using Npgsql;
using Testcontainers.PostgreSql;

namespace KbAis.Examples.PlanningPoker.Tests.Migrations;

public class MigrationTests : IAsyncLifetime {
    private CancellationTokenSource cts = new();

    [Fact]
    public async Task AllMigrationsShouldBeAppliedSuccessfully() {
        await using var planningPokerDataContainer = new PostgreSqlBuilder()
            .WithDatabase("dev").WithUsername("dev").WithPassword("dev")
            .WithPortBinding(5000, 5432)
            .Build();

        var cancellation = cts.Token;

        await planningPokerDataContainer.StartAsync(cancellation);

        var execResult = await planningPokerDataContainer
            .ExecScriptFileAsync("./_migrations/202401281048_init.sql", "dev", "dev", cancellation);

        execResult.ExitCode.Should().Be(0L);

        var planningPokerDataSource =
            new NpgsqlDataSourceBuilder(planningPokerDataContainer.GetConnectionString()).Build();

        await using var planningPokerDataConnection =
            await planningPokerDataSource.OpenConnectionAsync(cancellation);

    
        var tablesShouldBeCreate = new[] { "projects", "sprints", "task" };

        var tablesCreated = planningPokerDataConnection.Query<TableInfo>(Sql.GetTables).ToList();

        tablesCreated.ToList().Should().HaveCount(3);
        tablesCreated.Should().OnlyContain(table => tablesShouldBeCreate.Contains(table.Name));
    }
    
    public Task InitializeAsync() {
        return Task.CompletedTask;
    }

    public Task DisposeAsync() {
        cts.Dispose();

        return Task.CompletedTask;
    }

    private static class Sql {
        public const string GetTables =
            """
            SELECT
                tablename as Name
            FROM
                pg_catalog.pg_tables
            WHERE
                schemaname = 'poker';
            """;
    }

    private class TableInfo {
        public string Name { get; init; }
    }
}

internal static class PostgresContainerExtensions {
    public async static Task<ExecResult> ExecScriptFileAsync(
        this IContainer container,
        string scriptFilePath,
        string username,
        string database,
        CancellationToken cancellation = default
    ) {
        var scriptFile = new FileInfo(scriptFilePath);
        var scriptFileDirectory = string.Join("/", string.Empty, "tmp", Guid.NewGuid().ToString("D"), scriptFile.Name);

        await container.CopyAsync(source: scriptFile, target: scriptFileDirectory, Unix.FileMode644, cancellation)
            .ConfigureAwait(false);

        return await container
            .ExecAsync(
                new[] {
                    "psql",
                    "--username", username, "--dbname", database,
                    "--file", $"{scriptFileDirectory}/{scriptFile.Name}"
                },
                cancellation
            )
            .ConfigureAwait(false);
    }
}
