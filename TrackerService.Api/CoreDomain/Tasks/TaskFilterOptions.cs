using TrackerService.Core.CoreDomain.Tasks.Definitions;

namespace TrackerService.Api.CoreDomain.Tasks
{
    public class TaskFilterOptions : ITaskFilterOptions
    {
        public string CreatedById { get; set; }
        public string Status { get; set; }
        public string AssignedToId { get; set; }
    }
}
