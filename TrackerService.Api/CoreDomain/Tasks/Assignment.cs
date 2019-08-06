using System;
using TrackerService.Core.CoreDomain.Tasks;
using TrackerService.Core.CoreDomain.Tasks.Definitions;

namespace TrackerService.Api.CoreDomain.Tasks
{
    public class Assignment : ITaskAssignment
    {
        public AssignmentEntityType Type { get; set; }
        public string EntityId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
