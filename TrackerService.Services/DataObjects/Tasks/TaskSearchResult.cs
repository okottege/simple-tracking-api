using System.Collections.Generic;

namespace TrackerService.Services.DataObjects.Tasks
{
    public class TaskSearchResult
    {
        public List<TaskListItem> Tasks { get; set; }
        public long TotalNumberOfTasks { get; set; }
    }
}
