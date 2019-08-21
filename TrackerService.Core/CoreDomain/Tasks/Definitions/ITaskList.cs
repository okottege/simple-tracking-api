using System.Collections.Generic;

namespace TrackerService.Core.CoreDomain.Tasks.Definitions
{
    public interface ITaskList
    {
        List<ITask> Tasks { get; }
        int TotalNumberOfRecords { get; }
    }
}