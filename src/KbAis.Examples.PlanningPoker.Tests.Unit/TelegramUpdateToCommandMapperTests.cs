using FluentAssertions;
using KbAis.Examples.PlanningPoker.Runner.Application.Projects;
using KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KbAis.Examples.PlanningPoker.Tests.Unit;

[Trait("Category", "Functional")]
public class TelegramUpdateToCommandMapperTests {
    [Theory]
    [MemberData(nameof(TelegramUpdateRouterInput.Updates), MemberType = typeof(TelegramUpdateRouterInput))]
    public async Task Router_should_create_command(Update u) {
        var updCtx = new UpdateContext { BotId = 1L, Update = u };

        updCtx.MapToCommand().Should().BeOfType(typeof(RegisterNewProjectCommand));
    }
}

public class TelegramUpdateRouterInput {
    public static IEnumerable<object[]> Updates = new List<object[]> {
        // TODO: Return args enumerable
        new object[] { CreateFacilitatorSetOnBotAddedToGroup() }
    };

    private static Update CreateFacilitatorSetOnBotAddedToGroup() {
        return new() {
            Message = new() {
                Chat = new() { Type = ChatType.Group, Title = "Sample" },
                // TODO: Move to fixture
                From = new() { Username = "Sample" },
                NewChatMembers = [ new() { IsBot = true, Id = 1L } ]
            }
        };
    }
}
