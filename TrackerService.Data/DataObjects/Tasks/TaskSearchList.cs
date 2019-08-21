using System.Collections.Generic;
using TrackerService.Core.CoreDomain.Tasks.Definitions;

namespace TrackerService.Data.DataObjects.Tasks
{
    public class TaskSearchList : ITaskList
    {
        public List<ITask> Tasks { get; set; }
        public int TotalNumberOfRecords { get; set; }
    }
}
