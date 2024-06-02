using KbAis.Examples.PlanningPoker.Runner.Application.Core;
using KbAis.Examples.PlanningPoker.Runner.Application.Projects.UseCases;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KbAis.Examples.PlanningPoker.Runner.Infrastructure.Telegram;

public static class TgUpdateRouter {
    private delegate bool TgUpdatePredicate(TgUpdateContext tgUpdate);

    private delegate ICommand CommandFactory(Update update);

    private static readonly List<(TgUpdatePredicate Predicate, CommandFactory Command)> Predicates = [
        ( // Register a new project on bot adding to the project group
            ctx => ctx is { Message.Type: MessageType.GroupCreated } || BotHasBeenAdded(ctx),
            upd => new RegisterProjectCommand(upd.Message ?? throw new Exception("An invalid update"))
        ),
        // If all predicates return false, then route the update to the default handler to discard.
        (_ => true, upd => new DefaultCommand(upd))
    ];
    
    public static ICommand MapToCommand(this TgUpdateContext ctx) =>
        Predicates.First(p => p.Predicate(ctx)).Command(ctx.Update);

    private static bool BotHasBeenAdded(TgUpdateContext ctx) =>
        ctx.Message?.NewChatMembers?.Any(x => x.Id == ctx.BotId) ?? false;
}
