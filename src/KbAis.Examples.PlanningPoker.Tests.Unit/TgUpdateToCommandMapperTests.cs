using FluentAssertions;
using KbAis.Examples.PlanningPoker.Runner.Application.Projects.UseCases;
using KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KbAis.Examples.PlanningPoker.Tests.Unit;

[Trait("Category", "Functional")]
public class TgUpdateToCommandMapperTests {
    [Theory(DisplayName = "Маппер должен возвратить команду соответсвующую обновлению")]
    [MemberData(nameof(TgUpdateRouterInput.Updates), MemberType = typeof(TgUpdateRouterInput))]
    public void Mapper_should_return_correct_command(
        Type expectedCommandType, Update update
    ) {
        var updCtx = new TgUpdateContext { BotId = 1L, Update = update };

        updCtx.MapToCommand().Should().BeOfType(expectedCommandType);
    }
}

public class TgUpdateRouterInput {
    public static IEnumerable<object[]> Updates = new List<object[]> {
        RegisterProjectAndFacilitatorOnBotAdding()
    };

    private static object[] RegisterProjectAndFacilitatorOnBotAdding() => [
        typeof(RegisterNewProjectCommand), new Update { Message = new() {
            Chat = new() { Type = ChatType.Group, Title = "Sample" },
            // TODO: Move to fixture
            From = new() { Username = "Sample" },
            NewChatMembers = [ new() { IsBot = true, Id = 1L } ]
        } },
    ];
}
