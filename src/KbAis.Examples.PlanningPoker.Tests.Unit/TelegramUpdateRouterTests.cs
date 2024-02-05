using Telegram.Bot.Types;

namespace KbAis.Examples.PlanningPoker.Tests.Unit;

public class TelegramUpdateRouterTests {
    [Theory]
    [MemberData(nameof(TelegramUpdateRouterInput.Updates), MemberType = typeof(TelegramUpdateRouterInput))]
    public async Task Router_should_create_command(Update upd) {

    }
}

public class TelegramUpdateRouterInput {
    public static IEnumerable<object[]> Updates = new List<object[]> {
        new object[] { CreateFacilitatorSetOnBotAddedToGroup() }
    };

    private static Update CreateFacilitatorSetOnBotAddedToGroup() {
        return new() {

        };
    }
}
