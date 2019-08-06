using System;
using System.Collections.Generic;

namespace TrackerService.Core.CoreDomain.Tasks
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
    }
}