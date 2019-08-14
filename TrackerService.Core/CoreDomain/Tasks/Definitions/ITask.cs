using System;
using System.Collections.Generic;

namespace TrackerService.Core.CoreDomain.Tasks.Definitions
{
    public interface ITask : IAuditableEntity
    {
        string TaskId { get; }
        string Title { get; }
        string Description { get; }
        DateTime? DueDate { get; }
        TaskStatus Status { get; }
        TaskType Type { get; }
        ITask Parent { get; }
        List<ITask> Children { get; }
        List<ITaskAssignment> Assignments { get; }
        List<ITaskContextItem> ContextItems { get; }
        List<ITaskDependency> Dependencies { get; }
    }
}