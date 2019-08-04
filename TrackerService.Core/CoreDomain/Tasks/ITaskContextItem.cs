using System;

namespace TrackerService.Core.CoreDomain.Tasks
{
    public interface ITaskContextItem
    {
        string ContextKey { get; } 
        string ContextValue { get; }
        DateTime CreatedDate { get; }
        string CreatedBy { get; }
    }
}