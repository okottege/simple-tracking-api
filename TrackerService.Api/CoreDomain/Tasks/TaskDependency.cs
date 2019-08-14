using System;
using TrackerService.Core.CoreDomain.Tasks;
using TrackerService.Core.CoreDomain.Tasks.Definitions;

namespace TrackerService.Api.CoreDomain.Tasks
{
    public class TaskDependency : ITaskDependency
    {
        public string DependsOnTaskId { get; set; }
        public DependencyType Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
