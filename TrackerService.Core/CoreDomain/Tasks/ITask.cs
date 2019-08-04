using System;
using System.Collections.Generic;

namespace TrackerService.Core.CoreDomain.Tasks
{
    public interface ITask
    {
        string TaskId { get; }
        string Title { get; }
        string Description { get; }
        bool? Resolved { get; }
        DateTime? DueDate { get; }
        TaskStatus Status { get; }
        TaskType Type { get; }
        ITask Parent { get; }
        List<ITask> Children { get; }
        List<ITaskAssignment> Assignments { get; }
        List<ITaskContextItem> ContextItems { get; }
        string CreatedBy { get; }
        DateTime CreatedDate { get; }
        string ModifiedBy { get; }
        DateTime ModifiedDate { get; }
    }
}