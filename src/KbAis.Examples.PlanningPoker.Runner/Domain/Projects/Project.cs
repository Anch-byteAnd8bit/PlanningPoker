using KbAis.Examples.PlanningPoker.Runner.Domain.Members;

namespace KbAis.Examples.PlanningPoker.Runner.Domain.Projects;

public class Project {
    public static Project StartNew(
        ProjectChat chat, Member facilitator
    ) {
        return new() {
            
        };
    }
}

    public class Sprint {
    
}

public class SprintTask {

    
}

public class ProjectForDB
{
    public long id { get; init; }
    public DateTime started_at { get; init; }
    public string name { get; set; }
    public string description { get; set; }
    public long chatId { get; init; }

}

public class SprintForDB
{
    public long id { get; init; }
    public long project_id { get; init; }
    public int iteration { get; init; }
    public DateTime started_at { get; init; }
    public DateTime finished_at { get; init; }

}

public class TaskForDB
{
    public long id { get; init; }
    public long sprint_id { get; init; }
    public string name { get; set; }
    public string description { get; init; }

}

public class MemberForDB
{
    public long user_id { get; init; }
    public long project_id { get; init; }
}

public class outbox_table
{
    public long id { get; init; }
    public string textmessage { get; init; }
}
