using System;

namespace TrackerService.Core.CoreDomain.Tasks.Definitions
{
    public interface ITaskDependency
    {
        string DependsOnTaskId { get; }
        DependencyType Type { get; }
        DateTime CreatedDate { get; }
        string CreatedBy { get; }
    }
}