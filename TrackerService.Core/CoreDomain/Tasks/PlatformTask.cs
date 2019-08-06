using System;
using System.Collections.Generic;
using TrackerService.Core.CoreDomain.Tasks.Definitions;

namespace TrackerService.Core.CoreDomain.Tasks
{
    public class PlatformTask : ITask
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public TaskType Type { get; set; }
        public ITask Parent { get; set; }
        public List<ITask> Children { get; set; }
        public List<ITaskAssignment> Assignments { get; set; }
        public List<ITaskContextItem> ContextItems { get; set; }
    }
}
