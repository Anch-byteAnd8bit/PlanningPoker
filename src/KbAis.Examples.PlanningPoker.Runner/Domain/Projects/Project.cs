using KbAis.Examples.PlanningPoker.Runner.Application.Core;
using KbAis.Examples.PlanningPoker.Runner.Domain.Shared;
using StronglyTypedIds;

namespace KbAis.Examples.PlanningPoker.Runner.Domain.Projects;

[StronglyTypedId]
public readonly partial struct ProjectId;

/// <summary>
/// Проект, для которого проводится Planning Poker сессия.
/// </summary>
public class Project : Entity<ProjectId> {
    /// <summary>
    /// Чат в Telegram, который привязан к проведению Planning Poker-сессий.
    /// </summary>
    public ChatInfo Chat { get; private init; }

    private Project(ChatInfo chat) {
        Chat = chat;
    }

    /// <summary>
    /// Начать новый проект с указанными чатом Telegram.
    /// </summary>
    public static Project StartNew(ChatInfo chat) {
        return new(chat: chat);
    }

    public static class Errors {
        public const string AlreadyRegisteredByChat = "Project with such chat Identifier already registered";
    }
}

public class Sprint;

public class SprintTask;
