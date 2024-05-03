using KbAis.Examples.PlanningPoker.Runner.Application.Core;
using KbAis.Examples.PlanningPoker.Runner.Domain.Shared;
using StronglyTypedIds;

namespace KbAis.Examples.PlanningPoker.Runner.Domain.Projects;

[StronglyTypedId]
public readonly partial struct ProjectId;

public class Project : Entity<ProjectId> {
    public ChatInfo Chat { get; private init; }

    private Project(ChatInfo chat) {
        Chat = chat;
    }

    public static Project StartNew(ChatInfo chat) {
        return new(chat: chat);
    }

    public static class Errors {
        public const string AlreadyRregisteredByChat = "Project with such chat Identifier already registered";
    }
}

public class Sprint;

public class SprintTask;
